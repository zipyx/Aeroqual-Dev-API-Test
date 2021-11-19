using ApiLibrary.Models;
using JsonNet.ContractResolvers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ApiLibrary.Data
{
    public static class Migrate
    {
        public static void Seedit(string jsonData,
                            IServiceProvider serviceProvider)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver()
            };

            People? people = JsonConvert.DeserializeObject<People>(jsonData, settings);

            using (
             var serviceScope = serviceProvider
               .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApiContext>();
                if (context.People.Any())
                {
                    context.AddRange(people);
                    context.SaveChanges();
                }
            }
        }
    }
}
