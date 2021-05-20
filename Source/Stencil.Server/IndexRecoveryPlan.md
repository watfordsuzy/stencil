# Stencil Ticketing System Index Recovery Plan
- Author: christopherw@suzy.com
- Date: 2021-05-19

This document contains the plan to recover the `stencil_demo` index after the
mistaken release of the Stencil Ticketing System (STS) without applying a
migration to the ElasticSearch index.

## Abstract
During feature planning for the next frontend release of STS it was discovered
that the backend does not properly retrieve ticket comments by their author.
Root Cause Analysis determined that a schema migration was not applied to 
ElasticSearch when migrating the data schema.

## Background
The STS design was extended to include a new _Ticket Comment_ entity. This
entity is mapped many-to-one with the existing _Ticket_ entity. This entity
also has a many-to-one mapping with the existing _Account_ entity. These two
relationships are maintained via`GUID`s. A new STS Data Schema version (v3) was
created to reflect this change in the design, and a migration was created to
accomplish the schema upgrade. Work was completed to extend the backend to
support the new entity.

A code review was performed and completed. Unit tests of the endpoint passed
during CI testing. Integration testing has not been implemented yet for this
feature. QA performed manual testing using the SDK based on the feature 
specification. The feature was approved and the branch was merged into `main`.
CI passed on `main` and CD triggered a deployment to the production
environment.

During feature planning for the next frontend release, usage of the SDK
revealed that ticket comments could not be retrieved for a given author.
Testing of this new release of STS did not capture this bug, as ElasticSearch
retrieved the ticket comments per ticket correctly. At the time of the
release, there were no frontend components which needed to retrieve ticket
comments by their author, and testing of the SDK did not include this query.

A review of the SQL database determined that the data was inserted correctly.
A review of the ElasticSearch index determined that the data was inserted
correctly, albeit without the correct mapping. Inspecting the mapping for
`ticketcomments` revealed that ElasticSearch performed automatic mapping
because no schema migration was performed. The automatic mapping performed by
ElasticSearch determined that the `commenter_id` property should be an
analyzable string. Analyzable strings are not a compatible mapping for the
`GUID` datatype.

During the upgrade from Version 2 to Version 3 of the STS Data Schema, the
critical step of upgrading ElasticSearch mappings was not completed. The
Version 3 migration only included the steps to upgrade the Azure SQL Database
schema.

## Extent of Condition
Analysis of STS and the bug has determined the following:
1. Users have made heavy usage of _Ticket Comments_ after the feature release.
2. The Azure SQL Database is not affected.
3. The ElasticSearch Index `stencil_demo` is affected.
4. The mapping for _Ticket Comment_ is unusable in its current form.

### Diff of Actual vs Desired Index State
```diff
 {
     "ticketcomments": {
         "properties": {
             "ticket_comment_id": {
-                "type": "string"
+                "type": "string",
+                "index": "not_analyzed"
             },
             "ticket_id": {
                 "type": "string",
                 "index": "not_analyzed"
             },
             "commenter_id": {
-                "type": "string"
+                "type": "string",
+                "index": "not_analyzed"
             },
             "commented_on_utc": {
-                "type": "date",
-                "format": "strict_date_optional_time||epoch_millis"
+                "type": "date"
             },
             "ticket_comment": {
                 "type": "string"
             },
             "account_name": {
-                "type": "string"
+                "type": "string",
+                "fields": {
+                    "account_name": {
+                        "type": "string",
+                        "index": "analyzed"
+                    },
+                    "sort": {
+                        "type": "string",
+                        "analyzer": "case_insensitive"
+                    }
+                }
             },
             "account_email": {
-                "type": "string"
+                "type": "string",
+                "fields": {
+                    "account_email": {
+                        "type": "string",
+                        "index": "analyzed"
+                    },
+                    "sort": {
+                        "type": "string",
+                        "analyzer": "case_insensitive"
+                    }
+                }
             }
         }
     }
 }
```

## Recovery Plan
1. Create `stencil_demo_current` alias and point to `stencil_demo`.
2. Deploy a new app configuration which uses the `stencil_demo_current` alias.
3. Update the STS Data Schema version (v3) to include the Index changes.
4. Deploy the v3 STS Data Schema to a new `stencil_demo_v3` ElasticSearch index.
5. Reindex `stencil_demo` onto `stencil_demo_v3` (mark the current time).
6. Atomically rotate the `stencil_demo_current` alias to point to `stencil_demo_v3`.
7. Resynchronize all entities sync'd after the time marked in step 6

### 1. Create `stencil_demo_current` alias and point to `stencil_demo`
The following action will create the new alias:
```
PS> $body = '{"actions":[{"add":{"index":"stencil_demo","alias":"stencil_demo_current"}}]}'
PS> .\Invoke-Elastic.ps1 -ElasticHost:$ElasticHost -Credential:$Credential -Body:$body -Method POST -Resource '_aliases'
```

- Actor: christopherw@suzy.com
- Status: Completed on 2021-05-20T13:51:00-04:00

### 2. Deploy a new app configuration which uses the `stencil_demo_current` alias.
Update `localAppSettings.config`:
```diff
-    <add key="Stencil-ES-INDEX" value="stencil_demo" />
+    <add key="Stencil-ES-INDEX" value="stencil_demo_current" />
```

- Actor: christopherw@suzy.com
- Status: Completed on 2021-05-20T13:52:00-04:00

### 3. Update the STS Data Schema version (v3) to include the Index changes.
Update `0003_AddTicketComments.cs`:
```diff
         protected override void UpIndex(ElasticClient client)
         {
-            // CAW: I forgot to do this...woops
+            client.Map<sdk.TicketComment>(p =>
+                p.Index(this.IndexName)
+                 .Type(DocumentNames.TicketComment)
+                 .AutoMap()
+                 .Properties(props => props
+                     .String(s => s
+                         .Name(n => n.ticket_comment_id)
+                         .Index(FieldIndexOption.NotAnalyzed)
+                     ).String(s => s
+                         .Name(n => n.ticket_id)
+                         .Index(FieldIndexOption.NotAnalyzed)
+                     ).String(s => s
+                         .Name(n => n.commenter_id)
+                         .Index(FieldIndexOption.NotAnalyzed)
+                     ).String(m => m
+                         .Name(t => t.account_name)
+                         .Fields(f => f
+                                 .String(s => s.Name(n => n.account_name)
+                                 .Index(FieldIndexOption.Analyzed))
+                                 .String(s => s
+                                     .Name(n => n.account_name.Suffix("sort"))
+                                     .Analyzer("case_insensitive"))
+                         )
+                     ).String(m => m
+                         .Name(t => t.account_email)
+                         .Fields(f => f
+                                 .String(s => s.Name(n => n.account_email)
+                                 .Index(FieldIndexOption.Analyzed))
+                                 .String(s => s
+                                     .Name(n => n.account_email.Suffix("sort"))
+                                     .Analyzer("case_insensitive"))
+                         )
+                     )
+                 )
+             ).ThrowIfUnsuccessful();
         }
```

Create `Scripts\Elastic\index-v3-schema.json` with the new complete schema via `ElasticMappingGenerator.exe`.

### 4. Deploy the v3 STS Data Schema to a new `stencil_demo_v3` ElasticSearch index.
The following action will create the new index and apply the v3 schema:
```
PS> $body = [System.IO.File]::ReadAllText('Scripts\Elastic\index-v3-schema.json')
PS> .\Invoke-Elastic.ps1 -ElasticHost:$ElasticHost -Credential:$Credential -Body:$body -Method POST -Resource 'stencil_demo_v3'
```

- Actor: christopherw@suzy.com
- Status: Completed on 2021-05-20T14:02:00-04:00

### 5. Reindex `stencil_demo` onto `stencil_demo_v3` (mark the current time).
The following action will begin a reindex operation from `stencil_demo` to `stencil_demo_v3`:
```
PS> $now = [DateTimeOffset]::UtcNow
PS> $body = '{"source":{"index":"stencil_demo"},"dest":{"index":"stencil_demo_v3"}}'
PS> .\Invoke-Elastic.ps1 -ElasticHost:$ElasticHost -Credential:$Credential -Body:$body -Method POST -Resource '_reindex?wait_for_completion=false'
```
Follow along with the re-indexing via:
```
PS> .\Invoke-Elastic.ps1 -ElasticHost:$ElasticHost -Credential:$Credential '_tasks/?pretty&detailed=true&actions=*reindex'
```

- Actor: christopherw@suzy.com
- Status: In Progress on 2021-05-20T14:13:00-04:00