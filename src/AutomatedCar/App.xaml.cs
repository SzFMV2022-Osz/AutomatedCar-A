namespace AutomatedCar
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.Models.NPC;
    using AutomatedCar.Models.Route;
    using AutomatedCar.ViewModels;
    using AutomatedCar.Views;
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class App : Application
    {
        private static readonly string OVAL = "oval";
        private static readonly string TEST_WORLD = "test_world";

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var world = this.CreateWorld(OVAL);
                desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel(world) };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public World CreateWorld(string map)
        {
            var world = World.Instance;

            this.CreateNPCcar(325, 800, "car_1_blue.png", 1,  world, map);

            if (map == TEST_WORLD)
            {
                this.CreateNPCPerson(1, world);
            }

            world.PopulateFromJSON($"AutomatedCar.Assets.{map}.json");

            this.AddControlledCarsTo(world);
            //this.DrawPath(world);

            return world;
        }

        // TODO: It's temporary and testing purposes only. Should be deleted in final version.
        private void DrawPath(World world)
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("AutomatedCar.Assets.test_world.csv"));

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] coordinate = line.Split(';');
                int x = int.Parse(coordinate[0]);
                int y = int.Parse(coordinate[1]);
                int speed = int.Parse(coordinate[2]);

                var circle = new Circle(x, y, "circle.png", 2);
                circle.Width = 4;
                circle.Height = 4;
                circle.ZIndex = 20;
                circle.Rotation = 45;

                world.AddObject(circle);
            }
        }

        private PolylineGeometry GetControlledCarBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            foreach (var i in stuff["objects"][0]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
        }
        
        private PolylineGeometry GetBoundaryBox(int id)
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            
            foreach (var i in stuff["objects"][id]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
        }

        private void CreateNPCcar(int x, int y, string filename, int typeID, World world, string map)
        {
            Route route = Route.CreateFromJson($"AutomatedCar.Assets.{map}.csv");
            var car = new NpcCar(route, filename);
            PolylineGeometry boundaryBox = this.GetBoundaryBox(typeID);
            car.Geometries.Add(boundaryBox);
            car.RawGeometries.Add(boundaryBox);

            car.SetCoordinates();
            car.Start();

            world.AddObject(car);
        }

        private void CreateNPCPerson(int typeID, World world)
        {
            Route route = Route.CreateFromJson($"AutomatedCar.Assets.pedestrian.csv");
            var pedestrian = new NpcPerson(route, "woman.png");
            PolylineGeometry boundaryBox = this.GetBoundaryBox(typeID);
            pedestrian.Geometries.Add(boundaryBox);
            pedestrian.RawGeometries.Add(boundaryBox);

            pedestrian.SetCoordinates();
            pedestrian.Start();

            world.AddObject(pedestrian);
        }

        private AutomatedCar CreateControlledCar(int x, int y, int rotation, string filename)
        {
            var controlledCar = new Models.AutomatedCar(x, y, filename);
            controlledCar.Geometry = this.GetControlledCarBoundaryBox();
            controlledCar.RawGeometries.Add(controlledCar.Geometry);
            controlledCar.Geometries.Add(controlledCar.Geometry);
            controlledCar.RotationPoint = new System.Drawing.Point(54, 120);
            controlledCar.Rotation = rotation;

            controlledCar.Start();

            return controlledCar;
        }

        private void AddControlledCarsTo(World world)
        {
            var controlledCar = this.CreateControlledCar(480, 1425, 0, "car_1_white.png");
            var controlledCar2 = this.CreateControlledCar(4250, 1420, -90, "car_1_red.png");

            world.AddControlledCar(controlledCar);
            world.AddControlledCar(controlledCar2);
        }
    }
}