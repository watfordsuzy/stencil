using Newtonsoft.Json;
using Stencil.Plugins.GitHub.Integration;
using Stencil.Plugins.GitHub.Models;
using Stencil.SDK;
using Stencil.SDK.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoadGenerator
{
    class Program
    {
        static Random s_random = new Random();
        static HttpClient s_httpClient = new HttpClient();
        static JsonSerializer s_serializer = new JsonSerializer();

        static async Task Main(string[] args)
        {
            var sdk = new StencilSDK(args[0]);
            int taskCount = Int32.Parse(args[1]);

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) => 
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

            Console.WriteLine($"[INFO] Generating load with {taskCount} tasks");

            var tasks = new List<Task>();
            for (int ii = 0; ii < taskCount; ++ii)
            {
                int account = s_random.Next(100);
                var authInfo = await sdk.Auth.LoginAsync(
                    new Stencil.SDK.Models.Requests.AuthLoginInput
                    {
                        user = $"account{account}@example.com",
                        password = $"account{account}",
                    });

                tasks.Add(GenerateLoadAsync(args[0], authInfo.item, cts.Token));
            }

            tasks.Add(GenerateCommitsAsync(args[0], cts.Token));

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                if (ex is AggregateException aggEx)
                {
                    ex = aggEx.Flatten();
                }

                Console.WriteLine($"[ERROR] {ex}");
            }
        }

        private static async Task GenerateCommitsAsync(string baseUrl, CancellationToken token)
        {
            try
            {
                await GenerateCommitsImplAsync(baseUrl, token);
            }
            catch (OperationCanceledException)
            {
                /* CAW: Ignored */
            }
            catch
            {
                Console.WriteLine("[ERROR] Could not generate commits due to an error.");
                throw;
            }
        }

        private static async Task GenerateCommitsImplAsync(string baseUrl, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var sdk = new StencilSDK(baseUrl);
                int account = s_random.Next(100);
                var authInfo = await sdk.Auth.LoginAsync(
                    new Stencil.SDK.Models.Requests.AuthLoginInput
                    {
                        user = $"account{account}@example.com",
                        password = $"account{account}",
                    });

                sdk = new StencilSDK(authInfo.item.api_key, authInfo.item.api_secret, baseUrl);

                var tickets = await sdk.Ticket.GetTicketByReportedByIDAsync(authInfo.item.account_id, take: 50);
                if (tickets.items.Count > 0)
                {
                    var ticket = RandomItem(tickets.items);
                    Console.WriteLine($"[VERBOSE] {authInfo.item.first_name} is pushing a commit for {ticket.ticket_id}...");

                    var @ref = Guid.NewGuid().ToString("N");
                    await HttpPostEventWebHookAsync(
                        $@"{baseUrl}/github/webhook",
                        new GitHubPushEvent
                        {
                            @ref = @ref,
                            base_ref = Guid.NewGuid().ToString("N"),
                            before = Guid.NewGuid().ToString("N"),
                            after= Guid.Empty.ToString("N"),
                            created = true,
                            pusher = new GitHubCommitAuthor
                            {
                                name = $"{authInfo.item.first_name} {authInfo.item.last_name}",
                                email = authInfo.item.email,
                            },
                            commits = new[]
                            {
                                new GitHubCommit
                                {
                                    id = @ref,
                                    timestamp = DateTimeOffset.UtcNow.ToString(),
                                    author = new GitHubCommitAuthor
                                    {
                                        name = $"{authInfo.item.first_name} {authInfo.item.last_name}",
                                        email = authInfo.item.email,
                                    },
                                    url = $@"https://github.com/STS/STS/blob/{@ref}",
                                    distinct = true,
                                    message = $"Work [{ticket.ticket_id}]",
                                    added = new List<string>(),
                                    modified = new List<string>(),
                                    removed = new List<string>(),
                                },
                            },
                            repository = new GitHubRepository
                            {
                                name = "STS",
                                git_url = @"https://github.com/STS/STS.git",
                            },
                        },
                        token);

                    await Task.Delay(TimeSpan.FromSeconds(s_random.Next(5, 30)), token);
                }
                else
                {
                    Console.WriteLine($"[VERBOSE] {authInfo.item.first_name} has nothing to work on...");
                    await Task.Delay(TimeSpan.FromSeconds(s_random.Next(5, 10)), token);
                }
            }
        }

        private static async Task HttpPostEventWebHookAsync(string url, GitHubPushEvent gitHubPushEvent, CancellationToken token)
        {
            string sekret = @"how much wood could a woodchuck chuck";

            byte[] jsonUtf8Bytes;
            byte[] signature;
            using (var ms = new MemoryStream())
            {
                using (var writer = new JsonTextWriter(new StreamWriter(ms, Encoding.UTF8, 2048, leaveOpen: true)))
                {
                    s_serializer.Serialize(writer, gitHubPushEvent);
                    writer.Flush();
                }

                ms.Position = 0;
                jsonUtf8Bytes = ms.ToArray();

                using var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(sekret));
                signature = hmac.ComputeHash(jsonUtf8Bytes);
            }
            
            using var content = new ByteArrayContent(jsonUtf8Bytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            content.Headers.Add(GitHubAssumptions.HEADER_EVENT_NAME, GitHubPushEvent.EventName);
            content.Headers.Add(GitHubAssumptions.HEADER_DELIVERY, Guid.NewGuid().ToString());
            content.Headers.Add(GitHubAssumptions.HEADER_SIGNATURE, $"{GitHubAssumptions.SIGNATURE_PREFIX}{ToHex(signature)}");

            var response = await s_httpClient.PostAsync(url, content, token);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] GitHubPushEvent not accepted {response.StatusCode}");
            }

            string ToHex(byte[] bytes)
                => String.Join("", bytes.Select(bb => bb.ToString("x2")));
        }

        private static async Task GenerateLoadAsync(string baseUrl, AccountInfo accountInfo, CancellationToken token)
        {
            try
            {
                await GenerateLoadImplAsync(baseUrl, accountInfo, token);
            }
            catch (OperationCanceledException)
            {
                /* CAW: Ignored */
            }
            catch
            {
                Console.WriteLine("[ERROR] Could not generate actions due to an error.");
                throw;
            }
        }

        private static async Task GenerateLoadImplAsync(string baseUrl, AccountInfo accountInfo, CancellationToken token)
        {
            var sdk = new StencilSDK(accountInfo.api_key, accountInfo.api_secret, baseUrl);

            var products = (await sdk.Product.Find(take: 20)).items;
            var ownedProducts = products.Where(pp => pp.product_owner_id == accountInfo.account_id).ToArray();

            while (!token.IsCancellationRequested)
            {
                double value = s_random.NextDouble();

                // 60% of the time, create a ticket and add some comments for a while
                if (value < 0.60)
                {
                    Console.WriteLine($"[VERBOSE] {accountInfo.first_name} is filing a new ticket...");

                    var affectedProducts = Enumerable.Range(0, s_random.Next(2) + 1)
                                                     .Select(_ => RandomItem(products))
                                                     .Distinct()
                                                     .ToArray();

                    var ticket = await sdk.Ticket.CreateTicketAsync(
                        new Stencil.SDK.Models.Ticket
                        {
                            ticket_title = $"Problum with {String.Join(" and ", affectedProducts.Select(pp => pp.product_name))}",
                            ticket_description = $"I get this error code {Guid.NewGuid()}",
                            ticket_type = Stencil.SDK.Models.TicketType.Bug,
                        });

                    // Wait a bit after making the ticket before adding a comment
                    await Task.Delay(TimeSpan.FromSeconds(s_random.Next(5, 60)), token);

                    // Add a few comments
                    int commentCount = s_random.Next(5) + 1;
                    for (int ii = 0; ii < commentCount; ++ii)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        await sdk.TicketComment.CreateTicketCommentAsync(
                            new Stencil.SDK.Models.TicketComment
                            {
                                ticket_id = ticket.item.ticket_id,
                                ticket_comment = $"Hello? Is anyone working on my {ticket.item.ticket_title}",
                            });

                        await Task.Delay(TimeSpan.FromSeconds(s_random.Next(20, 60)), token);
                    }
                }
                else if (value < 0.8)
                {
                    const int take = 10;
                    if (ownedProducts.Length > 0)
                    {
                        Console.WriteLine($"[VERBOSE] {accountInfo.first_name} is assigning a product to a ticket...");

                        // 20% of the time, pick a product you own and add it to random tickets talking about it
                        var product = RandomItem(ownedProducts);

                        var tickets = await sdk.Ticket.Find(take: take, keyword: product.product_name);
                        if (tickets.items.Count == take && s_random.NextDouble() < 0.2)
                        {
                            // If we've got more tickets, randomly skip the first set and take a lot more
                            var moreTickets = await sdk.Ticket.Find(skip: tickets.items.Count, take: take * 3, keyword: product.product_name);
                            if (moreTickets.success && moreTickets.items.Count > 0)
                            {
                                tickets = moreTickets;
                            }
                        }

                        for (int ii = 0; ii < tickets.items.Count; ++ii)
                        {
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }

                            var ticketId = RandomItem(tickets.items, tt => tt.ticket_id);
                            await sdk.AffectedProduct.CreateAffectedProductAsync(new Stencil.SDK.Models.AffectedProduct
                            {
                                ticket_id = ticketId,
                                product_id = product.product_id,
                            });
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[VERBOSE] {accountInfo.first_name} is just hanging out...");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(s_random.Next(20, 100)), token);
                }
                else
                {
                    Console.WriteLine($"[VERBOSE] {accountInfo.first_name} is closing a ticket...");

                    // Otherwise, find a ticket of yours and close it
                    var tickets = await GetOpenTicketsAsync(accountInfo.account_id);
                    if (tickets.success && tickets.items.Count > 0)
                    {
                        var ticket = RandomItem(tickets.items);

                        await sdk.TicketComment.CreateTicketCommentAsync(new Stencil.SDK.Models.TicketComment
                        {
                            ticket_id = ticket.ticket_id,
                            ticket_comment = "Oh, I figured this out sorry!",
                        });

                        ticket.ticket_status = Stencil.SDK.Models.TicketStatus.Closed;

                        await sdk.Ticket.UpdateTicketAsync(ticket.ticket_id, ticket);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(s_random.Next(20, 100)), token);

                    async Task<ListResult<Stencil.SDK.Models.Ticket>> GetOpenTicketsAsync(Guid account_id)
                    {
                        int skip = 0, take = 10;
                        var tickets = await sdk.Ticket.GetTicketByReportedByIDAsync(account_id);
                        if (tickets.success)
                        {
                            // While we have tickets, but none of them are open...
                            while (tickets.items.Count > 0
                                    && !tickets.items.Any(tt => tt.ticket_status != Stencil.SDK.Models.TicketStatus.Closed))
                            {
                                skip += take;
                                tickets = await sdk.Ticket.GetTicketByReportedByIDAsync(account_id, skip: skip, take: take);
                            }
                        }

                        return tickets;
                    }
                }
            }
        }

        private static TResult RandomItem<TItem, TResult>(IList<TItem> items, Func<TItem, TResult> selector)
            => selector(RandomItem(items));

        private static TItem RandomItem<TItem>(IList<TItem> items)
            => items[s_random.Next(items.Count)];
    }
}
