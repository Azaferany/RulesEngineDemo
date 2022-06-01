// See https://aka.ms/new-console-template for more information


using Newtonsoft.Json;
using RulesEngine.Models;

while (true)
{
    string text = System.IO.File.ReadAllText(@"/Users/almas/Desktop/ConsoleApp1/ConsoleApp1/workflow.json");
    var workflowRules = JsonConvert.DeserializeObject<Workflow[]>(text);

    var reSettings = new ReSettings()
    {
        CustomTypes = new[] { typeof(WageResult) }
    };

    var rulesEngine = new RulesEngine.RulesEngine(workflowRules, reSettings);
    

    var user = new RuleParameter("User", new { Id = 64624, Wage = 0.2, MaxWage = 5000 , PanelType = 1});
    var client = new RuleParameter("Client", new { Id = 4534534, Wage = 0.3, MaxWage = 2000});
    var payment = new RuleParameter("Payment", new { Id = 4534534, Amount = 50000});
    
    var results = await rulesEngine.ExecuteAllRulesAsync("Wage", user, client, payment);

    var max= results.Select(x => x.ActionResult.Output as WageResult).Select(x => x?.Priority).Max();
    var result = results.SingleOrDefault(x => (x.ActionResult.Output as WageResult)?.Priority == max);
    
    Console.WriteLine(JsonConvert.SerializeObject(result.ActionResult.Output));
    Console.ReadKey();
}

public class WageResult
{
    public double Wage { get; set; }
    public int Priority { get; set; }
}