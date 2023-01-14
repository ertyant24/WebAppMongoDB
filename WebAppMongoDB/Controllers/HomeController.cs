using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using WebAppMongoDB.Entities;
using WebAppMongoDB.Models;

namespace WebAppMongoDB.Controllers
{
    public class HomeController : Controller
    {

        MongoClient client;
        IMongoDatabase database;
        IMongoCollection<Player> playerCollection;

		public HomeController()
		{
			client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("MyPlayerDB");
            playerCollection = database.GetCollection<Player>("Players");
		}

		public IActionResult Index()
        {
            //MongoClient client = new MongoClient("mongodb://localhost:27017");
            //IMongoDatabase database = client.GetDatabase("MyPlayerDB");
            //IMongoCollection<Player> playercollection = database.GetCollection<Player>("Players");

            return View(playerCollection.AsQueryable().ToList());
        }

		public IActionResult Create()
		{
			return View();
		}

        [HttpPost]
		public IActionResult Create(PlayerCreateViewModel model)
		{
            //MongoClient client = new MongoClient("mongodb://localhost:27017");
            //IMongoDatabase database = client.GetDatabase("MyPlayerDB");
            //IMongoCollection<Player> playercollection = database.GetCollection<Player>("Players");

            if (ModelState.IsValid)
            {
                Player player = new Player()
                {
                    Id = ObjectId.GenerateNewId(),
                    FullName = model.FullName,
                    ShoeSize = model.ShoeSize,
                    Team = model.Team,
                    IsActive = model.IsActive
                };

                playerCollection.InsertOne(player);

                return RedirectToAction(nameof(Index));
            }
            
			return View(model);
		}

		public IActionResult Edit(string id)
		{
			//MongoClient client = new MongoClient("mongodb://localhost:27017");
			//IMongoDatabase database = client.GetDatabase("MyPlayerDB");
			//IMongoCollection<Player> playercollection = database.GetCollection<Player>("Players");


			ObjectId objectId = ObjectId.Parse(id);

            Player player = playerCollection.AsQueryable().FirstOrDefault(x => x.Id == objectId);

            PlayerEditViewModel model = new PlayerEditViewModel();

            model.FullName = player.FullName;
            model.Team = player.Team;
            model.IsActive = player.IsActive;
            model.ShoeSize = player.ShoeSize;

			return View(model);
		}

        [HttpPost]
		public IActionResult Edit(string id, PlayerEditViewModel model)
		{

			//MongoClient client = new MongoClient("mongodb://localhost:27017");
			//IMongoDatabase database = client.GetDatabase("MyPlayerDB");
			//IMongoCollection<Player> playercollection = database.GetCollection<Player>("Players");

			if (ModelState.IsValid)
            {
                ObjectId objectId = ObjectId.Parse(id);
               Player player = playerCollection.AsQueryable().FirstOrDefault(x => x.Id == objectId);
                
               
                player.FullName = model.FullName;
                player.IsActive = model.IsActive;
                player.ShoeSize = model.ShoeSize;
                player.Team = model.Team;

                playerCollection.ReplaceOne(x => x.Id == objectId, player);

                return RedirectToAction(nameof(Index));
            }

			return View(model);
		}

		public IActionResult Delete(string id)
		{
            //MongoClient client = new MongoClient("mongodb://localhost:27017");
            //IMongoDatabase database = client.GetDatabase("MyPlayerDB");
            //IMongoCollection<Player> playerCollection = database.GetCollection<Player>("Players");

            ObjectId objectId = ObjectId.Parse(id);

            Player player = playerCollection.AsQueryable().FirstOrDefault(x => x.Id == objectId);

            playerCollection.DeleteOne(x => x.Id == objectId);

                return RedirectToAction(nameof(Index));

		}



		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}