using System;
using System.Globalization;
using System.IO;
using System.Linq;
using BookShop.Data;
using BookShop.Models;


namespace BookShop.Client
{
    class BookShopClient
    {
        static void Main()
        {
            var context = new BookShopContext();
            //var count = context.Categories.Count();
            //Console.WriteLine(count);

            using (var reader = new StreamReader("authors.txt"))
            {
                var line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null)
                {
                    var data = line.Split(new[] { ' ' }, 2);
                    string firstName = data[0];
                    string lastName = data[1];
                    context.Authors.Add(new Author()
                    {
                        FirstName = firstName,
                        LastName = lastName
                    });
                    line = reader.ReadLine();
                    context.SaveChanges();
                }
            }

            using (var categoryReader = new StreamReader("categories.txt"))
            {
                var line = categoryReader.ReadLine();
                line = categoryReader.ReadLine();
                while (line != null)
                {
                    context.Categories.Add(new Category()
                    {
                        Name = line
                    });
                    line = categoryReader.ReadLine();
                    context.SaveChanges();
                }
            }


            Random rnd = new Random();
            var authors = context.Authors.ToList();
            var categories = context.Categories.ToList();
            CultureInfo provider = CultureInfo.InvariantCulture;
            using (var bookReader = new StreamReader("books.txt"))
            {
                var line = bookReader.ReadLine();
                line = bookReader.ReadLine();
                while (line != null)
                {
                    var data = line.Split(new[] { ' ' }, 6);
                    var authorIndex = rnd.Next(0, authors.Count);
                    var author = authors[authorIndex];
                    var categoryIndex = rnd.Next(0, categories.Count);
                    var category = categories[categoryIndex];
                    var edition = (EditionType)int.Parse(data[0]);
                    var releaseDate = DateTime.ParseExact(data[1], "d/m/yyyy", provider);
                    var copies = int.Parse(data[2]);
                    var price = Double.Parse(data[3]);
                    var ageRestriction = int.Parse(data[4]);
                    var title = data[5];
                    context.Books.Add(new Book()
                    {
                        Author = author,
                        Title = title,
                        Edition = edition,
                        ReleaseDate = releaseDate,
                        Copies = copies,
                        Price = price,
                        AgeRestriction = ageRestriction
                    });
                    context.SaveChanges();
                    var book = context.Books.FirstOrDefault(b => b.Title == title);
                    book.Categories.Add(category);
                    line = bookReader.ReadLine();
                }
            }
        }
    }
}
