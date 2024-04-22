using Newtonsoft.Json;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public static class ToDoItemsSynchronizationService
    {
        public async static Task SynchronizeToDoItems()
        {
            Guid lastEventId = Guid.Empty;

            while (true)
            {
                var url = $"https://localhost:7040/feed?lastEventId={lastEventId}&seconds=10";
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var newEventId = GetLastEventId(responseBody);

                        if (newEventId != Guid.Empty)
                        {
                            lastEventId = newEventId;
                        }

                        File.WriteAllText("ToDoItemsCurrentSnapshot.json", responseBody);
                    }
                    else
                    {
                        await Task.Delay(1000);
                    }
                }
            }
        }

        private static Guid GetLastEventId(string responseBody)
        {
            var toDos = JsonConvert.DeserializeObject<List<ToDo>>(responseBody);

            if (toDos == null || !toDos.Any())
            {
                return Guid.Empty;
            }

            return toDos.Last().EventId;
        }
    }
}
