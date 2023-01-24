using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Tracking.Client;

HttpClient client = new();

client.BaseAddress = new Uri("https://localhost:7071");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

HttpResponseMessage response = await client.GetAsync("api/issue/all");
response.EnsureSuccessStatusCode();

if (response.IsSuccessStatusCode)
{
    bool keepRunning = true;
    int choice = 0;
    while(keepRunning)
    {
        Console.WriteLine();
        Console.WriteLine("-----------Issue interface-----------"); 
        Console.WriteLine("1. View all issues");
        Console.WriteLine("2. Get issue details by Id");
        Console.WriteLine("3. Create an issue");
        Console.WriteLine("4. Update an issue");
        Console.WriteLine("5. Delete an issue");
        Console.WriteLine("100. Close issue interface");
        choice = int.Parse(Console.ReadLine());
        IssueDto issue1 = null;

        switch(choice)
        {
            case 1:
                await IssueList();
                break;

            case 2:
                await IssueDetails();
                break;

            case 3:
                await CreateIssue();
                break;

            case 4:
                await UpdateIssue();
                break;

            case 5:
                await DeleteIssue();
                break;

            case 100:
                Console.WriteLine("!!!!!! CLIENT CONSOLE IS CLOSED !!!!!!!!");
                keepRunning = false;
                break;


        }
        async Task IssueList()
        {
            HttpResponseMessage response = await client.GetAsync("api/issue/all");
            response.EnsureSuccessStatusCode();
            var issues = await response.Content.ReadFromJsonAsync<IEnumerable<IssueDto>>();

            foreach (var issue in issues)
            {
                Console.WriteLine(issue.Id + ". " + issue.Title);
            }
        }

        async Task IssueDetails()
        {

            HttpResponseMessage response = await client.GetAsync("api/issue/all");
            response.EnsureSuccessStatusCode();
            var issues = await response.Content.ReadFromJsonAsync<IEnumerable<IssueDto>>();


            foreach (var issue in issues)
            {
                Console.WriteLine(issue.Id+". "+ issue.Title);
            }
            Console.WriteLine("Choose Id from above");
            int choosenId = int.Parse(Console.ReadLine());
            issue1 = issues.Where(i => i.Id == choosenId).FirstOrDefault();

            Console.WriteLine("ID: "+issue1.Id+
                "\nTitle: "+issue1.Title+
                "\nDescription: "+issue1.Description +
                "\nPriority: " + issue1.Priority+
                "\nIssue Type: " + issue1.IssueType+
                "\nCreated: " + issue1.Created);
        }

        async Task  CreateIssue()
        {
            Console.WriteLine("-----------Creating Issue------------");
            int input = 0;
            IssueDto issue = new(); 

            Console.Write("Enter issue title: ");
            issue.Title = Console.ReadLine();

            Console.Write("Enter issue description: ");
            issue.Description = Console.ReadLine();

            Console.Write("Enter issue priority(1 = Low, 2 = Medium, 3 = High): ");
            input = int.Parse(Console.ReadLine());
            issue.Priority = input-1 == 0 ? Priority.Low
                : input-1 == 1 ? Priority.Medium
                : Priority.High;

            Console.Write("Enter issue type(1=Feature, 2 = Bug, 3 = Documentation): ");
            input = int.Parse(Console.ReadLine());
            issue.IssueType = input-1 == 0 ? IssueType.Feature 
                : input-1 == 1 ? IssueType.Bug 
                : IssueType.Documentation;

            issue.Created = DateTime.Now;

            var stringPayload = JsonConvert.SerializeObject(issue);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            response = await client.PostAsync("api/issue/Create", httpContent);
            response.EnsureSuccessStatusCode();
            
            if (!response.IsSuccessStatusCode)
                Console.WriteLine(response.StatusCode);

        }

        async Task UpdateIssue()
        {
            await IssueList();

            Console.WriteLine();
            Console.Write("Select issue to update using id: ");
            int id = int.Parse(Console.ReadLine());

            int detail = 0;
            IssueDto issue = new();

            Console.Write("Enter issue title: ");
            issue.Title = Console.ReadLine();

            Console.Write("Enter issue description: ");
            issue.Description = Console.ReadLine();

            Console.Write("Enter issue priority(1 = Low, 2 = Medium, 3 = High): ");
            detail = int.Parse(Console.ReadLine());
            issue.Priority = detail - 1 == 0 ? Priority.Low
                : detail - 1 == 1 ? Priority.Medium
                : Priority.High;

            Console.Write("Enter issue type(1=Feature, 2 = Bug, 3 = Documentation): ");
            detail = int.Parse(Console.ReadLine());
            issue.IssueType = detail - 1 == 0 ? IssueType.Feature
                : detail - 1 == 1 ? IssueType.Bug
                : IssueType.Documentation;

            issue.Created = DateTime.Now;
            issue.Id = id;

            var stringPayload = JsonConvert.SerializeObject(issue);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            response = await client.PutAsync($"api/issue/Update/{id}", httpContent);
            response.EnsureSuccessStatusCode();


            if (response.IsSuccessStatusCode)
                Console.WriteLine("!!!! Issue updated !!!");
            else
                Console.WriteLine(response.StatusCode);

            Console.WriteLine();


        }

        async Task DeleteIssue()
        {
            await IssueList();

            Console.WriteLine(); 
            Console.Write("Select issue to delete using id: ");
            int input = int.Parse(Console.ReadLine());

            response = await client.DeleteAsync($"api/issue/Delete/{input}");
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
                Console.WriteLine("!!!! Issue deleted !!!");
            else
                Console.WriteLine(response.StatusCode);

            Console.WriteLine();
        }
    }
}
else
{
    Console.WriteLine(response.StatusCode.ToString());
}

Console.ReadLine();
