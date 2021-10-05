using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RichTextFormatter {
    private StringBuilder builder = new StringBuilder();

    private bool colorOpened;
    private bool fontOpened;
    private bool sizeOpened;

    public RichTextFormatter CloseColor() {
        builder.Append("</color>");
        colorOpened = false;
        return this;
    }

    public RichTextFormatter Color(Color color) {
        string htmlColor = ColorUtility.ToHtmlStringRGB(color);
        if (colorOpened) {
            CloseColor();
        }
        builder.Append($"<color=#{htmlColor}>");
        colorOpened = true;
        return this;
    }

    public RichTextFormatter CloseFont() {
        builder.Append("</font>");
        fontOpened = false;
        return this;
    }

    //Mouse SDF
    public RichTextFormatter Font(string font) {
        if (fontOpened) {
            CloseFont();
        }
        builder.Append($"<font=\"{font}\">");
        fontOpened = true;
        return this;
    }

    public RichTextFormatter CloseSize() {
        builder.Append("</size>");
        sizeOpened = false;
        return this;
    }

    public RichTextFormatter Size(int size) {
        if (sizeOpened) {
            CloseSize();
        }
        builder.Append($"<size={size}>");
        sizeOpened = true;
        return this;
    }

    public RichTextFormatter Append(string text) {
        builder.Append(text);
        return this;
    }

    public RichTextFormatter AppendLocalized(string field) {
        string localized = Globals.Localization.Get(field);
        builder.Append(localized);
        return this;
    }

    public string Finalize() {
        if (colorOpened) CloseColor();
        if (fontOpened) CloseFont();
        if (sizeOpened) CloseSize();
        return builder.ToString();
    }
}