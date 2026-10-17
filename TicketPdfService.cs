using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Tourist
{
    public static class TicketPdfService
    {
        static TicketPdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static byte[] CreateTicket(EventInfo ev, string holderName, string ticketNo,
                                          int ticketIndex, int ticketCount, decimal pricePaid)
        {
            return Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Size(PageSizes.A5.Landscape());
                    page.Margin(28);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Border(1).BorderColor(Colors.Grey.Medium).Padding(20).Column(col =>
                    {
                        col.Spacing(8);

                        col.Item().Text("BLUE HORIZON  |  EXPLORE SOUTH AFRICA")
                                  .FontSize(10).Bold().FontColor(Colors.Green.Darken3);

                        col.Item().Text(ev.Name).FontSize(26).Bold();

                        col.Item().Column(c =>
                        {
                            c.Spacing(4);
                            c.Item().Text(t => { t.Span("Venue: ").SemiBold(); t.Span(ev.Venue); });
                            c.Item().Text(t => {
                                t.Span("Date & time: ").SemiBold();
                                t.Span(ev.DateText);
                            });
                            c.Item().Text(t => { t.Span("Ticket holder: ").SemiBold(); t.Span(holderName); });
                            c.Item().Text(t => { t.Span("Ticket: ").SemiBold(); t.Span(ticketIndex + " of " + ticketCount); });
                            c.Item().Text(t => { t.Span("Price paid: ").SemiBold(); t.Span(Money.Zar(pricePaid)); });
                        });

                        col.Item().PaddingTop(10).Background(Colors.Grey.Lighten3).Padding(10).Column(c =>
                        {
                            c.Item().Text("TICKET NUMBER").FontSize(9).FontColor(Colors.Grey.Darken2);
                            c.Item().Text(ticketNo).FontSize(22).Bold();
                        });

                        col.Item().Text("Present this ticket at the entrance. One ticket admits one guest.")
                                  .FontSize(9).FontColor(Colors.Grey.Darken2);
                    });
                });
            }).GeneratePdf();
        }
    }
}
