using SkiaSharp;
using System;
using System.Globalization;
using System.Threading.Tasks;
using TicketBox.Application.DTOs;
using TicketBox.Application.Interfaces;

public partial class TicketImageService:ITicketImageService
{
    public Task<byte[]> GenerateTicketImage(TicketImageDto ticket)
    {

        const int width = 1400;
        const int height = 860;

        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;

        // =========================================================
        // PALETTE (referans görselden örneklenmiştir)
        // =========================================================

        var pageBg = SKColor.Parse("#141416"); // koyu zemin
        var cardCream = SKColor.Parse("#FAF6F1"); // bilet zemini
        var accentRed = SKColor.Parse("#C6363A"); // logo / başlık kırmızısı
        var bannerRed = SKColor.Parse("#C6363A");
        var textDark = SKColor.Parse("#1A1A1A");
        var textGray = SKColor.Parse("#767A82");
        var hairline = SKColor.Parse("#E7E1D8");
        var watermarkTint = SKColor.Parse("#EFE9E0");

        var statusText = ticket.Status.ToString().ToUpperInvariant();
        var statusColor = statusText switch
        {
            "USED" => SKColor.Parse("#6B7280"),
            "CANCELLED" => SKColor.Parse("#DC2626"),
            "EXPIRED" => SKColor.Parse("#DC2626"),
            "PENDING" => SKColor.Parse("#D97706"),
            _ => SKColor.Parse("#16A34A"),
        };

        // =========================================================
        // GEOMETRY
        // =========================================================

        const float cardTop = 40f;
        const float cardBottom = height - 40f;
        const float cardLeft = 40f;
        const float cardRight = width - 40f;
        const float perfX = 900f;
        const float cornerR = 20f;
        const float padX = cardLeft + 56f;   // sol içerik başlangıcı
        const float mainRight = perfX - 56f;      // sol bölüm sağ sınırı
        const float stubPadX = perfX + 60f;      // kupon içerik başlangıcı
        const float stubRight = cardRight - 56f;

        // =========================================================
        // PAINTS
        // =========================================================

        using var fillCream = new SKPaint { Color = cardCream, IsAntialias = true };
        using var fillPageBg = new SKPaint { Color = pageBg, IsAntialias = true };
        using var fillRed = new SKPaint { Color = accentRed, IsAntialias = true };
        using var fillBanner = new SKPaint { Color = bannerRed, IsAntialias = true };
        using var fillStatus = new SKPaint { Color = statusColor, IsAntialias = true };
        using var fillWhite = new SKPaint { Color = SKColors.White, IsAntialias = true };
        using var fillWatermark = new SKPaint { Color = watermarkTint, IsAntialias = true };

        using var textDarkPaint = new SKPaint { Color = textDark, IsAntialias = true };
        using var textGrayPaint = new SKPaint { Color = textGray, IsAntialias = true };
        using var textRedPaint = new SKPaint { Color = accentRed, IsAntialias = true };
        using var textWhitePaint = new SKPaint { Color = SKColors.White, IsAntialias = true };

        using var iconStroke = new SKPaint
        {
            Color = textDark,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2.6f,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        using var hairlinePaint = new SKPaint
        {
            Color = hairline,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1.5f,
            IsAntialias = true
        };

        using var dashedPaint = new SKPaint
        {
            Color = SKColor.Parse("#1A1A1A"),
            StrokeWidth = 2f,
            IsAntialias = true,
            PathEffect = SKPathEffect.CreateDash(new float[] { 6, 6 }, 0)
        };

        using var shadowPaint = new SKPaint
        {
            Color = SKColors.Black.WithAlpha(90),
            IsAntialias = true,
            MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 22)
        };

        // =========================================================
        // FONTS
        // =========================================================

        static SKTypeface Tf(string name, SKFontStyle style) => SKTypeface.FromFamilyName(name, style);

        using var brandFont = new SKFont(Tf("Segoe UI", SKFontStyle.Bold), 27);
        using var brandChipFont = new SKFont(Tf("Segoe UI", SKFontStyle.Bold), 20);
        using var tagFont = new SKFont(Tf("Segoe UI", SKFontStyle.Bold), 18);
        using var eventFont = new SKFont(Tf("Segoe UI", SKFontStyle.Bold), 50);
        using var labelFont = new SKFont(Tf("Segoe UI", SKFontStyle.Bold), 13);
        using var valueFont = new SKFont(Tf("Segoe UI", SKFontStyle.Bold), 24);
        using var valueSmallFont = new SKFont(Tf("Segoe UI", SKFontStyle.Bold), 20);
        using var smallFont = new SKFont(Tf("Segoe UI", SKFontStyle.Normal), 15);
        using var serialFont = new SKFont(Tf("Consolas", SKFontStyle.Bold), 21);
        using var bannerFont = new SKFont(Tf("Segoe UI", SKFontStyle.Bold), 22);

        // =========================================================
        // BACKGROUND + SHADOW
        // =========================================================

        canvas.Clear(pageBg);

        var shadowRect = new SKRoundRect(
            new SKRect(cardLeft + 6, cardTop + 10, cardRight - 2, cardBottom + 10), cornerR, cornerR);
        canvas.DrawRoundRect(shadowRect, shadowPaint);

        // =========================================================
        // MAIN CARD + STUB CARD (independently rounded -> natural notch)
        // =========================================================

        var mainCard = new SKRoundRect(new SKRect(cardLeft, cardTop, perfX, cardBottom), cornerR, cornerR);
        var stubCard = new SKRoundRect(new SKRect(perfX, cardTop, cardRight, cardBottom), cornerR, cornerR);

        canvas.DrawRoundRect(mainCard, fillCream);
        canvas.DrawRoundRect(stubCard, fillCream);

        // dashed perforation between the straight edges of both cards
        canvas.DrawLine(perfX, cardTop + cornerR + 4, perfX, cardBottom - cornerR - 4, dashedPaint);

        // =========================================================
        // LOGO HELPER
        // =========================================================

        void DrawLogo(float x, float y)
        {
            canvas.DrawText("TICKET", x, y, SKTextAlign.Left, brandFont, textDarkPaint);
            float tw = brandFont.MeasureText("TICKET");
            float chipX = x + tw + 8;
            float boxW = brandChipFont.MeasureText("BOX") + 26;
            var chip = new SKRoundRect(new SKRect(chipX, y - 24, chipX + boxW, y + 7), 6, 6);
            canvas.DrawRoundRect(chip, fillRed);
            canvas.DrawText("BOX", chipX + boxW / 2, y, SKTextAlign.Center, brandChipFont, textWhitePaint);
        }

        void DrawSpaced(string text, float x, float y, SKTextAlign align, SKFont font, SKPaint paint, float spacing)
        {
            float total = 0;
            foreach (var ch in text) total += font.MeasureText(ch.ToString()) + spacing;
            total -= spacing;
            float cursor = align switch
            {
                SKTextAlign.Center => x - total / 2,
                SKTextAlign.Right => x - total,
                _ => x,
            };
            foreach (var ch in text)
            {
                canvas.DrawText(ch.ToString(), cursor, y, SKTextAlign.Left, font, paint);
                cursor += font.MeasureText(ch.ToString()) + spacing;
            }
        }

        // =========================================================
        // ICON HELPERS (simple line icons)
        // =========================================================

        void DrawCalendarIcon(float x, float y, float s)
        {
            var body = new SKRoundRect(new SKRect(x, y + s * 0.18f, x + s, y + s), 4, 4);
            canvas.DrawRoundRect(body, iconStroke);
            canvas.DrawLine(x, y + s * 0.42f, x + s, y + s * 0.42f, iconStroke);
            canvas.DrawLine(x + s * 0.28f, y + s * 0.02f, x + s * 0.28f, y + s * 0.3f, iconStroke);
            canvas.DrawLine(x + s * 0.72f, y + s * 0.02f, x + s * 0.72f, y + s * 0.3f, iconStroke);
        }

        void DrawPinIcon(float x, float y, float s)
        {
            var cx = x + s / 2;
            var cy = y + s * 0.36f;
            var r = s * 0.3f;
            canvas.DrawCircle(cx, cy, r, iconStroke);
            using var dot = new SKPaint { Color = textDark, IsAntialias = true };
            canvas.DrawCircle(cx, cy, r * 0.32f, dot);
            using var path = new SKPath();
            path.MoveTo(cx - r * 0.85f, cy + r * 0.55f);
            path.LineTo(cx, y + s);
            path.LineTo(cx + r * 0.85f, cy + r * 0.55f);
            canvas.DrawPath(path, iconStroke);
        }

        void DrawPersonIcon(float x, float y, float s)
        {
            var cx = x + s / 2;
            canvas.DrawCircle(cx, y + s * 0.24f, s * 0.2f, iconStroke);
            using var path = new SKPath();
            path.MoveTo(x + s * 0.1f, y + s);
            path.CubicTo(
                x + s * 0.1f, y + s * 0.58f,
                x + s * 0.9f, y + s * 0.58f,
                x + s * 0.9f, y + s);
            canvas.DrawPath(path, iconStroke);
        }

        void DrawIconRow(float rowY, Action<float, float, float> icon, string label, string value)
        {
            icon(padX, rowY - 30, 40);
            DrawSpaced(label, padX + 56, rowY - 8, SKTextAlign.Left, labelFont, textGrayPaint, 1.4f);
            canvas.DrawText(value, padX + 56, rowY + 20, SKTextAlign.Left, valueFont, textDarkPaint);
        }

        // =========================================================
        // MAIN CARD CONTENT
        // =========================================================

        DrawLogo(padX, cardTop + 74);
        DrawSpaced("DİJİTAL BİLET", mainRight, cardTop + 74, SKTextAlign.Right, tagFont, textDarkPaint, 1.6f);
        canvas.DrawLine(padX, cardTop + 100, mainRight, cardTop + 100, hairlinePaint);

        string eventName = ticket.EventName ?? "ETKİNLİK";
        if (eventName.Length > 22) eventName = eventName.Substring(0, 19) + "...";
        canvas.DrawText(eventName.ToUpperInvariant(), padX, cardTop + 200, SKTextAlign.Left, eventFont, textRedPaint);

        DrawIconRow(cardTop + 270, DrawCalendarIcon, "TARİH",
            ticket.EventDate.ToString("dd MMMM yyyy dddd - HH:mm"));
        DrawIconRow(cardTop + 365, DrawPinIcon, "MEKAN", ticket.EventLocation ?? "Mekan bilgisi yok");
        DrawIconRow(cardTop + 460, DrawPersonIcon, "SAHİBİ", ticket.Holder ?? "Misafir");

        // watermark: faint crowd silhouette above footer text
        canvas.Save();
        canvas.ClipRect(new SKRect(padX, cardTop + 610, mainRight, cardBottom - 60));
        float peakBase = cardBottom - 60;
        for (float px = padX - 20; px < mainRight + 40; px += 26)
        {
            using var path = new SKPath();
            float h = 26 + (float)(Math.Sin(px * 0.15) * 10 + 12);
            path.MoveTo(px, peakBase);
            path.LineTo(px + 8, peakBase - h);
            path.LineTo(px + 16, peakBase);
            path.Close();
            canvas.DrawPath(path, fillWatermark);
        }
        canvas.Restore();

        canvas.DrawText("Bu bilet devredilemez. Etkinlik kurallarını",
            padX, cardBottom - 44, SKTextAlign.Left, smallFont, textGrayPaint);
        canvas.DrawText("www.ticketbox.com.tr üzerinden okuyabilirsiniz.",
            padX, cardBottom - 22, SKTextAlign.Left, smallFont, textGrayPaint);

        // QR block (right side of main card)
        const float qrSize = 250f;
        float qrLeft = mainRight - qrSize;
        float qrTop = cardTop + 235;

        if (!string.IsNullOrEmpty(ticket.QRCode))
        {
            try
            {
                byte[] qrBytes = Convert.FromBase64String(ticket.QRCode);
                using var qrData = SKData.CreateCopy(qrBytes);
                using var qrImage = SKImage.FromEncodedData(qrData);

                if (qrImage != null)
                {
                    var qrRect = new SKRect(qrLeft, qrTop, qrLeft + qrSize, qrTop + qrSize);
                    canvas.DrawImage(qrImage, qrRect, new SKSamplingOptions(SKFilterMode.Nearest));
                }
            }
            catch
            {
                // geçersiz QR verisi — boş bırak
            }
        }

        float qrCenterX = qrLeft + qrSize / 2;
        DrawSpaced("SERİ NO", qrCenterX, qrTop + qrSize + 40, SKTextAlign.Center, labelFont, textGrayPaint, 1.4f);
        canvas.DrawText(ticket.SerialNumber ?? "UNKNOWN", qrCenterX, qrTop + qrSize + 70,
            SKTextAlign.Center, serialFont, textDarkPaint);

        // =========================================================
        // STUB CARD CONTENT
        // =========================================================

        canvas.Save();
        canvas.ClipRoundRect(stubCard, antialias: true);

        DrawLogo(stubPadX, cardTop + 74);

        float row1Y = cardTop + 190;
        DrawSpaced("KOLTUK", stubPadX, row1Y - 22, SKTextAlign.Left, labelFont, textGrayPaint, 1.4f);
        canvas.DrawText(string.IsNullOrEmpty(ticket.SeatNumber) ? "GENEL" : ticket.SeatNumber,
            stubPadX, row1Y + 14, SKTextAlign.Left, valueFont, textDarkPaint);
        canvas.DrawLine(stubPadX, row1Y + 42, stubRight, row1Y + 42, hairlinePaint);

        float row2Y = row1Y + 112;
        DrawSpaced("DURUM", stubPadX, row2Y - 22, SKTextAlign.Left, labelFont, textGrayPaint, 1.4f);
        canvas.DrawCircle(stubPadX + 9, row2Y + 8, 7, fillStatus);
        canvas.DrawText(statusText, stubPadX + 26, row2Y + 14, SKTextAlign.Left, valueSmallFont, textDarkPaint);
        canvas.DrawLine(stubPadX, row2Y + 42, stubRight, row2Y + 42, hairlinePaint);

        float row3Y = row2Y + 112;
        DrawSpaced("TARİH", stubPadX, row3Y - 22, SKTextAlign.Left, labelFont, textGrayPaint, 1.4f);
        canvas.DrawText(ticket.EventDate.ToString("dd.MM.yyyy"),
            stubPadX, row3Y + 14, SKTextAlign.Left, valueSmallFont, textDarkPaint);

        // bottom red banner (rounded only at the bottom, matching card corners)
        var bannerRadii = new[]
        {
            new SKPoint(0, 0), new SKPoint(0, 0),
            new SKPoint(cornerR, cornerR), new SKPoint(cornerR, cornerR)
        };
        var bannerRoundRect = new SKRoundRect();
        bannerRoundRect.SetRectRadii(new SKRect(perfX, cardBottom - 130, cardRight, cardBottom), bannerRadii);
        canvas.DrawRoundRect(bannerRoundRect, fillBanner);

        // faint crowd silhouette texture inside the banner
        canvas.Save();
        canvas.ClipRoundRect(bannerRoundRect, antialias: true);
        using (var bannerTexture = new SKPaint { Color = SKColors.White.WithAlpha(20), IsAntialias = true })
        {
            float baseline = cardBottom - 130 + 34;
            for (float px = perfX - 10; px < cardRight + 20; px += 22)
            {
                using var path = new SKPath();
                float h = 14 + (float)(Math.Sin(px * 0.2) * 6 + 8);
                path.MoveTo(px, baseline);
                path.LineTo(px + 7, baseline - h);
                path.LineTo(px + 14, baseline);
                path.Close();
                canvas.DrawPath(path, bannerTexture);
            }
        }
        canvas.Restore();

        canvas.DrawText("İYİ EĞLENCELER!", perfX + (cardRight - perfX) / 2, cardBottom - 58,
            SKTextAlign.Center, bannerFont, textWhitePaint);

        canvas.Restore(); // stub clip

        // =========================================================
        // EXPORT
        // =========================================================

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return Task.FromResult(data.ToArray());
    }
}