using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;
using Tourplaner.Infrastructure;
using Tourplaner.Models;

namespace Tourplaner.Reports
{
    public sealed class TourDocument : IDocument
    {
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public TourDocument(Tour model, byte[] image)
        {
            Assert.NotNull(model, nameof(model));
            Assert.NotNull(model.Route, nameof(model.Route));
            Assert.NotNull(model.Logs, nameof(model.Logs));

            this.model = model;
            this.image = image;
        }

        public void Compose(IContainer container)
        {
            container
                .PaddingHorizontal(50)
                .PaddingVertical(50)
                .Page(page =>
                {
                    page.Header().Element(ComposeDocumentHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer()
                        .AlignCenter()
                        .PageNumber("Page {number}");
                });
        }

        private void ComposeDocumentHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Item().Text($"Tourname: {model.Name}", TextStyle.Default.Size(20));
                    stack.Item().Text($"From: {model.Route.From}");
                    stack.Item().Text($"To: {model.Route.To}");

                    stack.Item().Text("Description:");
                    stack.Item().Text(model.Description);
                });

                row.ConstantColumn(100)
                    .Height(50)
                    .Placeholder();
            });
        }

        private void ComposeContent(IContainer container)
        {
            container
                .PaddingTop(10)
                .Page(page =>
                {
                    //page.Header()
                    //    .BorderBottom(1)
                    //    .Padding(5)
                    //    .Row(row =>
                    //    {
                    //        row.ConstantColumn(25).Padding(2).Text("#");
                    //        row.RelativeColumn(2).Padding(2).Text("Date");

                    //        row.RelativeColumn().Padding(2).Text("Distance");
                    //        row.RelativeColumn().Padding(2).Text("Speed");
                    //        row.RelativeColumn().Padding(2).Text("Breaks");
                    //        row.RelativeColumn().Padding(2).Text("Brawls");
                    //        row.RelativeColumn().Padding(2).Text("Abductions");
                    //        row.RelativeColumn().Padding(2).Text("Hobgoblins");
                    //        row.RelativeColumn().Padding(2).Text("UFO´s");
                    //        row.RelativeColumn().Padding(2).Text("Time");
                    //        row.RelativeColumn().Padding(2).Text("Rating");
                    //    });

                    page
                        .Content()
                        .Stack(stack =>
                        {
                            stack
                                .Item()
                                .BorderBottom(1)
                                .Padding(5)
                                .Row(row =>
                                {
                                    row.ConstantColumn(25).Padding(2).Text("#");
                                    row.RelativeColumn(2).Padding(2).Text("Date");

                                    row.RelativeColumn().Padding(2).Text("Distance");
                                    row.RelativeColumn().Padding(2).Text("Speed");
                                    row.RelativeColumn().Padding(2).Text("Breaks");
                                    row.RelativeColumn().Padding(2).Text("Brawls");
                                    row.RelativeColumn().Padding(2).Text("Abductions");
                                    row.RelativeColumn().Padding(2).Text("Hobgoblins");
                                    row.RelativeColumn().Padding(2).Text("UFO´s");
                                    row.RelativeColumn().Padding(2).Text("Time");
                                    row.RelativeColumn().Padding(2).Text("Rating");
                                });

                            foreach (TourLog log in model.Logs)
                            {
                                stack
                                    .Item()
                                    .BorderBottom(1)
                                    .BorderColor("CCC")
                                    .Padding(5)
                                    .Row(row =>
                                    {
                                        row.ConstantColumn(25).Padding(2).Text(model.Logs.IndexOf(log) + 1);
                                        row.RelativeColumn(2).Padding(2).Text(log.TourDate);

                                        row.RelativeColumn().Padding(2).Text(log.Distance);
                                        row.RelativeColumn().Padding(2).Text(log.AvgSpeed);
                                        row.RelativeColumn().Padding(2).Text(log.Breaks);
                                        row.RelativeColumn().Padding(2).Text(log.Brawls);
                                        row.RelativeColumn().Padding(2).Text(log.Abductions);
                                        row.RelativeColumn().Padding(2).Text(log.HobgoblinSightings);
                                        row.RelativeColumn().Padding(2).Text(log.UFOSightings);
                                        row.RelativeColumn().Padding(2).Text(log.TotalTime);
                                        row.RelativeColumn().Padding(2).Text(log.Rating);
                                    });
                            }

                            if (image != null)
                                stack.Item().Image(image);
                        });

                    page
                        .Footer()
                        .Stack(stack =>
                        {
                            stack.Item().Text($"Sum Time: {model.Logs.Sum(l => l.TotalTime)}");
                            stack.Item().Text($"Sum Distance: {model.Logs.Sum(l => l.Distance)}");
                        });
                });
        }

        private readonly Tour model;
        private readonly byte[] image;
    }
}
