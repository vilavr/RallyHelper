using System.Collections.Generic;
using Domain;
using DAL;

namespace DbOperations;

public class InsertStartData
{
    private readonly AppDbContext _context;
    private readonly Random _random = new Random();

    public InsertStartData(AppDbContext context)
    {
        _context = context;
    }

    public void InsertData()
    {
        if (!_context.ItemLocations.Any())
        {
            var locations = GetLocations();
            _context.ItemLocations.AddRange(locations);
        }

        if (!_context.ItemCategories.Any())
        {
            var categories = GetCategories();
            _context.ItemCategories.AddRange(categories);
        }

        _context.SaveChanges();

        if (!_context.Items.Any())
        {
            var items = GetItems();
            _context.Items.AddRange(items);
        }

        _context.SaveChanges();
    }

    private List<ItemLocation> GetLocations()
    {
        return new List<ItemLocation>
        {
            new ItemLocation { LocationName = "Central Warehouse Tallinn" },
            new ItemLocation { LocationName = "Tallinn East Depot" },
            new ItemLocation { LocationName = "Tallinn West Distribution Center" },
            new ItemLocation { LocationName = "Tallinn South Storage" },
            new ItemLocation { LocationName = "Tallinn North Logistics Hub" }
        };
    }

    private List<ItemCategory> GetCategories()
    {
        return new List<ItemCategory>
        {
            new ItemCategory { CategoryName = "Engine Parts" },
            new ItemCategory { CategoryName = "Transmission" },
            new ItemCategory { CategoryName = "Suspension" },
            new ItemCategory { CategoryName = "Electronics" },
            new ItemCategory { CategoryName = "Body & Frame" }
        };
    }

    private List<Item> GetItems()
    {
        var itemDetails = new Dictionary<int, List<string>>
        {
            { 1, new List<string> { "Piston", "Crankshaft", "Turbocharger", "Fuel Injector", "Oil Filter" }},
            { 2, new List<string> { "Clutch Disc", "Gearbox", "Drive Shaft", "Differential", "Flywheel" }},
            { 3, new List<string> { "Shock Absorber", "Coil Spring", "Strut", "Sway Bar", "Control Arm" }},
            { 4, new List<string> { "ECU", "Battery", "Alternator", "Starter Motor", "Headlight" }},
            { 5, new List<string> { "Fender", "Bumper", "Hood", "Door", "Windshield" }}
        };

        var items = new List<Item>();
        foreach (var location in _context.ItemLocations)
        {
            foreach (var categoryItems in itemDetails)
            {
                foreach (var itemName in categoryItems.Value)
                {
                    var optimalQuantity = _random.Next(5, 15);
                    var price = 50 + (optimalQuantity * location.Id); // Example price calculation

                    items.Add(new Item
                    {
                        ItemName = itemName,
                        CategoryId = categoryItems.Key,
                        LocationId = location.Id,
                        CurrentQuantity = optimalQuantity,
                        OptimalQuantity = optimalQuantity,
                        PricePerItem = price
                    });
                }
            }
        }
        return items;
    }
}
