using GadgetHub.Models;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.Data.DataSeed;

public class DataSeed
{
    private static readonly string _phone = "Phones";
    private static readonly string _laptop = "Laptops";
    private static readonly string _accessory = "Accessories";

    public static async Task PopulateAsync(ApplicationDbContext context,ILoggerFactory loggerFactory)
    {
        try
        {
            if (!await context.Categories.AnyAsync())
            {
                await context.Categories.AddRangeAsync(new List<Category>
                {
                    new Category{Name = _phone, Description = "Mobile phones, Tablets etc" },
                    new Category{Name = _laptop, Description = "Laptops, Notebooks etc" },
                    new Category{Name = _accessory, Description = "Headsets, Earpiece, Airpods, Chargers, Watches etc" }
                });

                await context.SaveChangesAsync();
            }

            if (!await context.Products.AnyAsync())
            {
                var categories = await context.Categories.ToListAsync();

                int phoneId = categories.First(x => x.Name == _phone).Id;
                int laptopId = categories.First(x => x.Name == _laptop).Id;
                int accessoryId = categories.First(x => x.Name == _accessory).Id;

                await context.Products.AddRangeAsync(new List<Product>
                {
                    new Product{Name = "Apple iPhone 12 Pro 6.1 RAM 6GB 512GB", CategoryId = phoneId, 
                        Description = "The Apple iPhone 12, released in October 2020, showcases a modern design with flat edges, glass front and back, and aluminum edges. It boasts a Super Retina XDR display in 6.1-inch and 5.4-inch sizes, delivering vibrant colors and high brightness. Powered by the A14 Bionic chip, it offers impressive performance and energy efficiency. Notably, the iPhone 12 supports 5G connectivity for faster internet speeds. Its dual-camera system with wide and ultra-wide lenses excels in photography, featuring Night mode and Deep Fusion technology. Overall, the iPhone 12 combines sleek design, powerful performance, and advanced camera capabilities.",
                    Price = 360000.50f, Image = "~/images/iphone12.webp", NewRelease = true, Ratings = 5},
                    new Product{Name = "Xiaomi Redmi 8 Original Global Version 4GB", CategoryId = phoneId, 
                        Description = "The Xiaomi Redmi 8, released in 2019, presents a budget-friendly smartphone option with a modern design. It features a 6.22-inch display with good visuals. The device is powered by a Qualcomm Snapdragon 439 processor, offering decent performance for everyday tasks. Its dual-camera system includes a primary and depth sensor, providing satisfactory photo quality. The Redmi 8 runs on MIUI, Xiaomi's custom Android skin. With its affordable price point, the Redmi 8 caters to users seeking basic smartphone features without breaking the bank.",
                    Price = 230000f, Image = "~/images/xiaomi-redmi-8.webp", NewRelease = true, Ratings = 3},
                    new Product{Name = "Apple Watch Series 1 Sport Case 38mm Black", CategoryId = accessoryId, 
                        Description = "The Apple Watch is a line of smartwatches renowned for their sleek design, health-focused features, and integration with iPhones. It features a square display with customizable watch faces and various sizes and materials. Key aspects include health monitoring capabilities like heart rate tracking, ECG (from Series 4 onwards), and blood oxygen monitoring (Series 6). The watch also handles notifications, calls, and messages, and supports apps for diverse tasks. Customization options include interchangeable bands. While battery life lasts about a day, the Apple Watch is an evolving accessory, with each series improving functionality and offering new features.",
                    Price = 160000f, Image = "~/images/apple-watch.webp", NewRelease = true, Ratings = 4},
                    new Product{Name = "Gaming Headset 32db Blackbuilt in mic", CategoryId = accessoryId, 
                        Description = "A gaming headset is a specialized accessory for gamers that enhances the gaming experience. It features a stylish design, comfortable fit, and often includes RGB lighting. The headset provides high-quality audio with immersive sound effects, aiding gameplay by offering precise audio cues. Surround sound technology helps players pinpoint directional sounds. Additionally, these headsets have built-in microphones for clear in-game communication.",
                    Price = 60000f, Image = "~/images/gaming-headset.webp", NewRelease = true, Ratings = 4},
                    new Product{Name = "Apple iPhone 11 Pro 6.1 RAM 6GB 512GB", CategoryId = phoneId,
                        Description = "The Apple iPhone 11, released in September 20, 2019, showcases a modern design with flat edges, glass front and back, and aluminum edges. It boasts a Super Retina XDR display in 6.1-inch and 5.4-inch sizes, delivering vibrant colors and high brightness. Powered by the A14 Bionic chip, it offers impressive performance and energy efficiency. Notably, the iPhone 12 supports 5G connectivity for faster internet speeds. Its dual-camera system with wide and ultra-wide lenses excels in photography, featuring Night mode and Deep Fusion technology. Overall, the iPhone 12 combines sleek design, powerful performance, and advanced camera capabilities.",
                    Price = 260000.50f, Image = "~/images/iphone11.jpeg", NewRelease = false, Ratings = 3},
                    new Product{Name = "Apple iPhone X Pro 6.1 RAM 6GB 512GB", CategoryId = phoneId,
                        Description = "The Apple iPhone X, released in November 3, 2017, showcases a modern design with flat edges, glass front and back, and aluminum edges. It boasts a Super Retina XDR display in 6.1-inch and 5.4-inch sizes, delivering vibrant colors and high brightness. Powered by the A14 Bionic chip, it offers impressive performance and energy efficiency. Notably, the iPhone 12 supports 5G connectivity for faster internet speeds. Its dual-camera system with wide and ultra-wide lenses excels in photography, featuring Night mode and Deep Fusion technology. Overall, the iPhone 12 combines sleek design, powerful performance, and advanced camera capabilities.",
                    Price = 290000.50f, Image = "~/images/iphonex.jpeg", NewRelease = false, Ratings = 3},
                    new Product{Name = "Lenovo Ideapad V15", CategoryId = laptopId,
                        Description = "The Lenovo laptop is a reliable computing device known for its performance and versatility. It comes in various models, offering options for different needs and budgets. Lenovo laptops typically feature modern designs, durable build quality, and a range of hardware configurations. They run on Windows operating systems and are equipped with powerful processors, ample RAM, and spacious storage options. Lenovo laptops are used for a variety of tasks, from productivity and entertainment to gaming and creative work. They are popular for their balance between performance, design, and value.",
                    Price = 310000.50f, Image = "~/images/lenovo.jpeg", NewRelease = true, Ratings = 3},
                    new Product{Name = "Hp EliteBook G590", CategoryId = laptopId,
                        Description = "HP laptops are renowned for their diverse range catering to various user needs. They feature stylish designs, robust build quality, and configurations suitable for different tasks and budgets. With Windows operating systems, these laptops offer dependable performance powered by capable processors, sufficient memory, and ample storage. HP laptops excel in productivity, entertainment, and casual gaming, making them versatile choices. Their combination of design, functionality, and performance has contributed to their popularity among users.",
                    Price = 290000.50f, Image = "~/images/hp.jpeg", NewRelease = true, Ratings = 4},
                    new Product{Name = "Dell Latitude 3189", CategoryId = laptopId,
                        Description = "Dell laptops are known for their reliability and performance. With a variety of models available, they cater to different user requirements. Dell laptops feature sturdy builds and modern designs, offering a range of hardware configurations to suit various tasks and budgets. They run on Windows operating systems and come equipped with powerful processors, ample RAM, and spacious storage options. Dell laptops are trusted for their efficiency in tasks like productivity, entertainment, and even gaming. Their reputation for quality, combined with their diverse offerings, makes them a popular choice among consumers.",
                    Price = 290000.50f, Image = "~/images/dell.jpeg", NewRelease = true, Ratings = 5},
                    new Product{Name = "Macbook Pro", CategoryId = laptopId,
                        Description = "MacBooks are Apple's line of premium laptops known for their sleek design, powerful performance, and seamless integration with the macOS ecosystem. They come in various models, offering different sizes and specifications. MacBooks are characterized by their high-resolution Retina displays, durable build quality, and efficient hardware configurations. They run on macOS, Apple's operating system, and are powered by advanced processors, ample RAM, and fast storage options. MacBooks are favored by professionals, creatives, and general users alike for their excellent performance in tasks ranging from productivity and content creation to software development and design.",
                    Price = 290000.50f, Image = "~/images/mac.jpeg", NewRelease = true, Ratings = 4},

                      new Product{Name = "Airpods", CategoryId = accessoryId,
                        Description = "Apple AirPods are wireless earbuds developed by Apple. They offer a seamless and convenient audio experience, connecting effortlessly to various Apple devices via Bluetooth. AirPods feature a sleek design and provide high-quality sound, while also incorporating features like touch controls for playback and Siri voice activation. With a charging case that extends battery life and promotes portability, AirPods have become a popular choice for users seeking wireless audio solutions that integrate seamlessly into the Apple ecosystem.",
                    Price = 84000f, Image = "~/images/airpods.jpeg", NewRelease = true, Ratings = 4},
                      new Product{Name = "USB Flash Drive 32GB", CategoryId = accessoryId,
                        Description = "Portable storage device that uses flash memory to store and transfer data. It connects to devices through a USB port and is commonly used for quickly transferring files, storing backups, and carrying data on-the-go. Flash drives come in various storage capacities and are compact, durable, and convenient for sharing and accessing files across different computers and devices.",
                    Price = 13000f, Image = "~/images/usb.jpeg", NewRelease = true, Ratings = 2},
                });

                await context.SaveChangesAsync();
            }
        }
        catch(Exception ex)
        {
            var logger = loggerFactory.CreateLogger<DataSeed>();

            logger.LogError("An error occurred. Details: {error}", ex.StackTrace);
        }

        
    }
}
