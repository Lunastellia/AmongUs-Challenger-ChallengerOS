using System;
using UnityEngine;

public class RainbowUtils
{
    private static readonly int BackColor = Shader.PropertyToID("_BackColor");
    private static readonly int BodyColor = Shader.PropertyToID("_BodyColor");
    private static readonly int VisorColor = Shader.PropertyToID("_VisorColor");


    public static Color Rainbow => new HSBColor(PP(0, 1, 0.3f), 1, 1).ToColor();
    public static Color RainbowShadow => Shadow(Rainbow);

    public static Color Ruby => new HSBColor_Ruby(PP(0.4f, 1, 0.3f), 1, 1).ToColor();
    public static Color RubyShadow => Shadow(Ruby);

    public static Color Amber => new HSBColor_Amber(PP(0.4f, 1, 0.3f), 1, 1).ToColor();
    public static Color AmberShadow => Shadow(Amber);

    public static Color Emerald => new HSBColor_Emerald(PP(0.4f, 1, 0.3f), 1, 1).ToColor();
    public static Color EmeraldShadow => Shadow(Emerald);

    public static Color Larimar => new HSBColor_Larimar(PP(0.4f, 1, 0.3f), 1, 1).ToColor();
    public static Color LarimarShadow => Shadow(Larimar);

    public static Color Sapphir => new HSBColor_Sapphir(PP(0.4f, 1, 0.3f), 1, 1).ToColor();
    public static Color SapphirShadow => Shadow(Sapphir);

    public static Color Quartz => new HSBColor_Quartz(PP(0.4f, 1, 0.3f), 1, 1).ToColor();
    public static Color QuartzShadow => Shadow(Quartz);



    public static float PP(float min, float max, float mul)
    {
        return min + Mathf.PingPong(Time.time * mul, max - min);
    }

    public static Color Shadow(Color color)
    {
        return new Color(color.r - 0.3f, color.g - 0.3f, color.b - 0.3f);
    }

    public static void SetRainbowVisor(Renderer rend)
    {
        rend.material.SetColor(BackColor, RainbowShadow);
        rend.material.SetColor(BodyColor, Rainbow);
        if (ChallengerMod.Challenger.LoverEvent)
        {
            rend.material.SetColor(VisorColor, ChallengerMod.ColorTable.CupidColor);
        }
        else
        {
            rend.material.SetColor(VisorColor, Palette.VisorColor);
        }

    }
    public static void SetRubyVisor(Renderer rend)
    {
        rend.material.SetColor(BackColor, RubyShadow);
        rend.material.SetColor(BodyColor, Ruby);

        if (ChallengerMod.Challenger.LoverEvent)
        {
            rend.material.SetColor(VisorColor, ChallengerMod.ColorTable.CupidColor);
        }
        else
        {
            rend.material.SetColor(VisorColor, Palette.VisorColor);
        }

    }
    public static void SetAmberVisor(Renderer rend)
    {
        rend.material.SetColor(BackColor, AmberShadow);
        rend.material.SetColor(BodyColor, Amber);

        if (ChallengerMod.Challenger.LoverEvent)
        {
            rend.material.SetColor(VisorColor, ChallengerMod.ColorTable.CupidColor);
        }
        else
        {
            rend.material.SetColor(VisorColor, Palette.VisorColor);
        }

    }
    public static void SetEmeraldVisor(Renderer rend)
    {
        rend.material.SetColor(BackColor, EmeraldShadow);
        rend.material.SetColor(BodyColor, Emerald);

        if (ChallengerMod.Challenger.LoverEvent)
        {
            rend.material.SetColor(VisorColor, ChallengerMod.ColorTable.CupidColor);
        }
        else
        {
            rend.material.SetColor(VisorColor, Palette.VisorColor);
        }

    }
    public static void SetLarimarVisor(Renderer rend)
    {
        rend.material.SetColor(BackColor, LarimarShadow);
        rend.material.SetColor(BodyColor, Larimar);

        if (ChallengerMod.Challenger.LoverEvent)
        {
            rend.material.SetColor(VisorColor, ChallengerMod.ColorTable.CupidColor);
        }
        else
        {
            rend.material.SetColor(VisorColor, Palette.VisorColor);
        }

    }
    public static void SetSapphirVisor(Renderer rend)
    {
        rend.material.SetColor(BackColor, SapphirShadow);
        rend.material.SetColor(BodyColor, Sapphir);

        if (ChallengerMod.Challenger.LoverEvent)
        {
            rend.material.SetColor(VisorColor, ChallengerMod.ColorTable.CupidColor);
        }
        else
        {
            rend.material.SetColor(VisorColor, Palette.VisorColor);
        }

    }
    public static void SetQuartzVisor(Renderer rend)
    {
        rend.material.SetColor(BackColor, QuartzShadow);
        rend.material.SetColor(BodyColor, Quartz);

        if (ChallengerMod.Challenger.LoverEvent)
        {
            rend.material.SetColor(VisorColor, ChallengerMod.ColorTable.CupidColor);
        }
        else
        {
            rend.material.SetColor(VisorColor, Palette.VisorColor);
        }

    }
    public static void SetNormalColorVisor(Renderer rend)
    {
        
         if (ChallengerMod.Challenger.LoverEvent)
        {
            rend.material.SetColor(VisorColor, ChallengerMod.ColorTable.CupidColor);
        }
        else
        {
            rend.material.SetColor(VisorColor, Palette.VisorColor);
        }
    }

    public static bool IsRainbow(int id)
    {
        try
        {
            return (int)Palette.ColorNames[id] == 999993;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsRuby(int id)
    {
        try
        {
            return (int)Palette.ColorNames[id] == 999994;
        }
        catch
        {
            return false;
        }
    }
    public static bool IsAmber(int id)
    {
        try
        {
            return (int)Palette.ColorNames[id] == 999995;
        }
        catch
        {
            return false;
        }
    }
    public static bool IsEmerald(int id)
    {
        try
        {
            return (int)Palette.ColorNames[id] == 999996;
        }
        catch
        {
            return false;
        }
    }
    public static bool IsLarimar(int id)
    {
        try
        {
            return (int)Palette.ColorNames[id] == 999997;
        }
        catch
        {
            return false;
        }
    }
    public static bool IsSapphir(int id)
    {
        try
        {
            return (int)Palette.ColorNames[id] == 999998;
        }
        catch
        {
            return false;
        }
    }
    public static bool IsQuartz(int id)
    {
        try
        {
            return (int)Palette.ColorNames[id] == 999999;
        }
        catch
        {
            return false;
        }
    }
    public static bool IsNormalColor(int id)
    {
        try
        {
            return (int)Palette.ColorNames[id] < 999993;
        }
        catch
        {
            return false;
        }
    }
}

[Serializable]

public struct HSBColor
{
    public float h;
    public float s;
    public float b;
    public float a;

    public HSBColor(float h, float s, float b, float a)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        this.a = a;
    }

    public HSBColor(float h, float s, float b)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        a = 1f;
    }

    public HSBColor(Color col)
    {
        var temp = FromColor(col);
        h = temp.h;
        s = temp.s;
        b = temp.b;
        a = temp.a;
    }

    public static HSBColor FromColor(Color color)
    {
        var ret = new HSBColor(0f, 0f, 0f, color.a);

        var r = color.r;
        var g = color.g;
        var b = color.b;

        var max = Mathf.Max(r, Mathf.Max(g, b));

        if (max <= 0)
        {
            return ret;
        }

        var min = Mathf.Min(r, Mathf.Min(g, b));
        var dif = max - min;

        if (max > min)
        {
            if (g == max)
            {
                ret.h = (b - r) / dif * 60f + 120f;
            }
            else if (b == max)
            {
                ret.h = (r - g) / dif * 60f + 240f;
            }
            else if (b > g)
            {
                ret.h = (g - b) / dif * 60f + 360f;
            }
            else
            {
                ret.h = (g - b) / dif * 60f;
            }

            if (ret.h < 0)
            {
                ret.h = ret.h + 360f;
            }
        }
        else
        {
            ret.h = 0;
        }

        ret.h *= 1f / 360f;
        ret.s = (dif / max) * 1f;
        ret.b = max;

        return ret;
    }

    public static Color ToColor(HSBColor hsbColor)
    {
        var r = hsbColor.b;
        var g = hsbColor.b;
        var b = hsbColor.b;
        if (hsbColor.s != 0)
        {
            var max = hsbColor.b;
            var dif = hsbColor.b * hsbColor.s;
            var min = hsbColor.b - dif;

            var h = hsbColor.h * 360f;

            if (h < 60f)
            {
                r = max;
                g = h * dif / 60f + min;
                b = min;
            }
            else if (h < 120f)
            {
                r = -(h - 120f) * dif / 60f + min;
                g = max;
                b = min;
            }
            else if (h < 180f)
            {
                r = min;
                g = max;
                b = (h - 120f) * dif / 60f + min;
            }
            else if (h < 240f)
            {
                r = min;
                g = -(h - 240f) * dif / 60f + min;
                b = max;
            }
            else if (h < 300f)
            {
                r = (h - 240f) * dif / 60f + min;
                g = min;
                b = max;
            }
            else if (h <= 360f)
            {
                r = max;
                g = min;
                b = -(h - 360f) * dif / 60 + min;
            }
            else
            {
                r = 0;
                g = 0;
                b = 0;
            }
        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), hsbColor.a);
    }

    public Color ToColor()
    {
        return ToColor(this);
    }

    public override string ToString()
    {
        return "H:" + h + " S:" + s + " B:" + b;
    }

    public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
    {
        float h, s;

        //check special case black (color.b==0): interpolate neither hue nor saturation!
        //check special case grey (color.s==0): don't interpolate hue!
        if (a.b == 0)
        {
            h = b.h;
            s = b.s;
        }
        else if (b.b == 0)
        {
            h = a.h;
            s = a.s;
        }
        else
        {
            if (a.s == 0)
            {
                h = b.h;
            }
            else if (b.s == 0)
            {
                h = a.h;
            }
            else
            {
                var angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
                while (angle < 0f)
                    angle += 360f;
                while (angle > 360f)
                    angle -= 360f;
                h = angle / 360f;
            }

            s = Mathf.Lerp(a.s, b.s, t);
        }

        return new HSBColor(h, s, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
    }

    
}


public struct HSBColor_Ruby
{
    public float h;
    public float s;
    public float b;
    public float a;

    public HSBColor_Ruby(float h, float s, float b, float a)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        this.a = a;
    }

    public HSBColor_Ruby(float h, float s, float b)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        a = 1f;
    }

    public HSBColor_Ruby(Color col)
    {
        var temp = FromColor(col);
        h = temp.h;
        s = temp.s;
        b = temp.b;
        a = temp.a;
    }

    public static HSBColor_Ruby FromColor(Color color)
    {
        var ret = new HSBColor_Ruby(0f, 0f, 0f, color.a);

        var r = color.r;
        var g = color.g;
        var b = color.b;

        var max = Mathf.Max(r, Mathf.Max(g, b));

        if (max <= 0)
        {
            return ret;
        }

        var min = Mathf.Min(r, Mathf.Min(g, b));
        var dif = max - min;

        if (max > min)
        {
            if (g == max)
            {
                ret.h = (b - r) / dif * 60f + 120f;
            }
            else if (b == max)
            {
                ret.h = (r - g) / dif * 60f + 240f;
            }
            else if (b > g)
            {
                ret.h = (g - b) / dif * 60f + 360f;
            }
            else
            {
                ret.h = (g - b) / dif * 60f;
            }

            if (ret.h < 0)
            {
                ret.h = ret.h + 360f;
            }
        }
        else
        {
            ret.h = 0;
        }

        ret.h *= 1f / 360f;
        ret.s = (dif / max) * 1f;
        ret.b = max;

        return ret;
    }

    public static Color ToColor(HSBColor_Ruby HSBColor_Ruby)
    {
        var r = HSBColor_Ruby.b;
        var g = HSBColor_Ruby.b;
        var b = HSBColor_Ruby.b;
        if (HSBColor_Ruby.s != 0)
        {
            var max = HSBColor_Ruby.b;
            var dif = HSBColor_Ruby.b * HSBColor_Ruby.s;
            var min = HSBColor_Ruby.b - dif;

            var h = HSBColor_Ruby.h * 360f;

            
                r = h * dif / 360f + min;
                g = min;
                b = min;
           
        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), HSBColor_Ruby.a);
    }

    public Color ToColor()
    {
        return ToColor(this);
    }

    public override string ToString()
    {
        return "H:" + h + " S:" + s + " B:" + b;
    }

    public static HSBColor_Ruby Lerp(HSBColor_Ruby a, HSBColor_Ruby b, float t)
    {
        float h, s;

        //check special case black (color.b==0): interpolate neither hue nor saturation!
        //check special case grey (color.s==0): don't interpolate hue!
        if (a.b == 0)
        {
            h = b.h;
            s = b.s;
        }
        else if (b.b == 0)
        {
            h = a.h;
            s = a.s;
        }
        else
        {
            if (a.s == 0)
            {
                h = b.h;
            }
            else if (b.s == 0)
            {
                h = a.h;
            }
            else
            {
                var angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
                while (angle < 0f)
                    angle += 360f;
                while (angle > 360f)
                    angle -= 360f;
                h = angle / 360f;
            }

            s = Mathf.Lerp(a.s, b.s, t);
        }

        return new HSBColor_Ruby(h, s, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
    }

   
}

public struct HSBColor_Amber
{
    public float h;
    public float s;
    public float b;
    public float a;

    public HSBColor_Amber(float h, float s, float b, float a)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        this.a = a;
    }

    public HSBColor_Amber(float h, float s, float b)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        a = 1f;
    }

    public HSBColor_Amber(Color col)
    {
        var temp = FromColor(col);
        h = temp.h;
        s = temp.s;
        b = temp.b;
        a = temp.a;
    }

    public static HSBColor_Amber FromColor(Color color)
    {
        var ret = new HSBColor_Amber(0f, 0f, 0f, color.a);

        var r = color.r;
        var g = color.g;
        var b = color.b;

        var max = Mathf.Max(r, Mathf.Max(g, b));

        if (max <= 0)
        {
            return ret;
        }

        var min = Mathf.Min(r, Mathf.Min(g, b));
        var dif = max - min;

        if (max > min)
        {
            if (g == max)
            {
                ret.h = (b - r) / dif * 60f + 120f;
            }
            else if (b == max)
            {
                ret.h = (r - g) / dif * 60f + 240f;
            }
            else if (b > g)
            {
                ret.h = (g - b) / dif * 60f + 360f;
            }
            else
            {
                ret.h = (g - b) / dif * 60f;
            }

            if (ret.h < 0)
            {
                ret.h = ret.h + 360f;
            }
        }
        else
        {
            ret.h = 0;
        }

        ret.h *= 1f / 360f;
        ret.s = (dif / max) * 1f;
        ret.b = max;

        return ret;
    }

    public static Color ToColor(HSBColor_Amber HSBColor_Amber)
    {
        var r = HSBColor_Amber.b;
        var g = HSBColor_Amber.b;
        var b = HSBColor_Amber.b;
        if (HSBColor_Amber.s != 0)
        {
            var max = HSBColor_Amber.b;
            var dif = HSBColor_Amber.b * HSBColor_Amber.s;
            var min = HSBColor_Amber.b - dif;

            var h = HSBColor_Amber.h * 360f;


            r = max;
            g = h * dif / 360f + min;
            b = min;

        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), HSBColor_Amber.a);
    }

    public Color ToColor()
    {
        return ToColor(this);
    }

    public override string ToString()
    {
        return "H:" + h + " S:" + s + " B:" + b;
    }

    public static HSBColor_Amber Lerp(HSBColor_Amber a, HSBColor_Amber b, float t)
    {
        float h, s;

        //check special case black (color.b==0): interpolate neither hue nor saturation!
        //check special case grey (color.s==0): don't interpolate hue!
        if (a.b == 0)
        {
            h = b.h;
            s = b.s;
        }
        else if (b.b == 0)
        {
            h = a.h;
            s = a.s;
        }
        else
        {
            if (a.s == 0)
            {
                h = b.h;
            }
            else if (b.s == 0)
            {
                h = a.h;
            }
            else
            {
                var angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
                while (angle < 0f)
                    angle += 360f;
                while (angle > 360f)
                    angle -= 360f;
                h = angle / 360f;
            }

            s = Mathf.Lerp(a.s, b.s, t);
        }

        return new HSBColor_Amber(h, s, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
    }


}
public struct HSBColor_Emerald
{
    public float h;
    public float s;
    public float b;
    public float a;

    public HSBColor_Emerald(float h, float s, float b, float a)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        this.a = a;
    }

    public HSBColor_Emerald(float h, float s, float b)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        a = 1f;
    }

    public HSBColor_Emerald(Color col)
    {
        var temp = FromColor(col);
        h = temp.h;
        s = temp.s;
        b = temp.b;
        a = temp.a;
    }

    public static HSBColor_Emerald FromColor(Color color)
    {
        var ret = new HSBColor_Emerald(0f, 0f, 0f, color.a);

        var r = color.r;
        var g = color.g;
        var b = color.b;

        var max = Mathf.Max(r, Mathf.Max(g, b));

        if (max <= 0)
        {
            return ret;
        }

        var min = Mathf.Min(r, Mathf.Min(g, b));
        var dif = max - min;

        if (max > min)
        {
            if (g == max)
            {
                ret.h = (b - r) / dif * 60f + 120f;
            }
            else if (b == max)
            {
                ret.h = (r - g) / dif * 60f + 240f;
            }
            else if (b > g)
            {
                ret.h = (g - b) / dif * 60f + 360f;
            }
            else
            {
                ret.h = (g - b) / dif * 60f;
            }

            if (ret.h < 0)
            {
                ret.h = ret.h + 360f;
            }
        }
        else
        {
            ret.h = 0;
        }

        ret.h *= 1f / 360f;
        ret.s = (dif / max) * 1f;
        ret.b = max;

        return ret;
    }

    public static Color ToColor(HSBColor_Emerald HSBColor_Emerald)
    {
        var r = HSBColor_Emerald.b;
        var g = HSBColor_Emerald.b;
        var b = HSBColor_Emerald.b;
        if (HSBColor_Emerald.s != 0)
        {
            var max = HSBColor_Emerald.b;
            var dif = HSBColor_Emerald.b * HSBColor_Emerald.s;
            var min = HSBColor_Emerald.b - dif;

            var h = HSBColor_Emerald.h * 360f;


            r = min;
            g = h * dif / 360f + min;
            b = min;

        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), HSBColor_Emerald.a);
    }

    public Color ToColor()
    {
        return ToColor(this);
    }

    public override string ToString()
    {
        return "H:" + h + " S:" + s + " B:" + b;
    }

    public static HSBColor_Emerald Lerp(HSBColor_Emerald a, HSBColor_Emerald b, float t)
    {
        float h, s;

        //check special case black (color.b==0): interpolate neither hue nor saturation!
        //check special case grey (color.s==0): don't interpolate hue!
        if (a.b == 0)
        {
            h = b.h;
            s = b.s;
        }
        else if (b.b == 0)
        {
            h = a.h;
            s = a.s;
        }
        else
        {
            if (a.s == 0)
            {
                h = b.h;
            }
            else if (b.s == 0)
            {
                h = a.h;
            }
            else
            {
                var angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
                while (angle < 0f)
                    angle += 360f;
                while (angle > 360f)
                    angle -= 360f;
                h = angle / 360f;
            }

            s = Mathf.Lerp(a.s, b.s, t);
        }

        return new HSBColor_Emerald(h, s, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
    }


}
public struct HSBColor_Larimar
{
    public float h;
    public float s;
    public float b;
    public float a;

    public HSBColor_Larimar(float h, float s, float b, float a)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        this.a = a;
    }

    public HSBColor_Larimar(float h, float s, float b)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        a = 1f;
    }

    public HSBColor_Larimar(Color col)
    {
        var temp = FromColor(col);
        h = temp.h;
        s = temp.s;
        b = temp.b;
        a = temp.a;
    }

    public static HSBColor_Larimar FromColor(Color color)
    {
        var ret = new HSBColor_Larimar(0f, 0f, 0f, color.a);

        var r = color.r;
        var g = color.g;
        var b = color.b;

        var max = Mathf.Max(r, Mathf.Max(g, b));

        if (max <= 0)
        {
            return ret;
        }

        var min = Mathf.Min(r, Mathf.Min(g, b));
        var dif = max - min;

        if (max > min)
        {
            if (g == max)
            {
                ret.h = (b - r) / dif * 60f + 120f;
            }
            else if (b == max)
            {
                ret.h = (r - g) / dif * 60f + 240f;
            }
            else if (b > g)
            {
                ret.h = (g - b) / dif * 60f + 360f;
            }
            else
            {
                ret.h = (g - b) / dif * 60f;
            }

            if (ret.h < 0)
            {
                ret.h = ret.h + 360f;
            }
        }
        else
        {
            ret.h = 0;
        }

        ret.h *= 1f / 360f;
        ret.s = (dif / max) * 1f;
        ret.b = max;

        return ret;
    }

    public static Color ToColor(HSBColor_Larimar HSBColor_Larimar)
    {
        var r = HSBColor_Larimar.b;
        var g = HSBColor_Larimar.b;
        var b = HSBColor_Larimar.b;
        if (HSBColor_Larimar.s != 0)
        {
            var max = HSBColor_Larimar.b;
            var dif = HSBColor_Larimar.b * HSBColor_Larimar.s;
            var min = HSBColor_Larimar.b - dif;

            var h = HSBColor_Larimar.h * 360f;


            r = min;
            g = max;
            b = h * dif / 360f + min;

        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), HSBColor_Larimar.a);
    }

    public Color ToColor()
    {
        return ToColor(this);
    }

    public override string ToString()
    {
        return "H:" + h + " S:" + s + " B:" + b;
    }

    public static HSBColor_Larimar Lerp(HSBColor_Larimar a, HSBColor_Larimar b, float t)
    {
        float h, s;

        //check special case black (color.b==0): interpolate neither hue nor saturation!
        //check special case grey (color.s==0): don't interpolate hue!
        if (a.b == 0)
        {
            h = b.h;
            s = b.s;
        }
        else if (b.b == 0)
        {
            h = a.h;
            s = a.s;
        }
        else
        {
            if (a.s == 0)
            {
                h = b.h;
            }
            else if (b.s == 0)
            {
                h = a.h;
            }
            else
            {
                var angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
                while (angle < 0f)
                    angle += 360f;
                while (angle > 360f)
                    angle -= 360f;
                h = angle / 360f;
            }

            s = Mathf.Lerp(a.s, b.s, t);
        }

        return new HSBColor_Larimar(h, s, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
    }


}

public struct HSBColor_Sapphir
{
    public float h;
    public float s;
    public float b;
    public float a;

    public HSBColor_Sapphir(float h, float s, float b, float a)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        this.a = a;
    }

    public HSBColor_Sapphir(float h, float s, float b)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        a = 1f;
    }

    public HSBColor_Sapphir(Color col)
    {
        var temp = FromColor(col);
        h = temp.h;
        s = temp.s;
        b = temp.b;
        a = temp.a;
    }

    public static HSBColor_Sapphir FromColor(Color color)
    {
        var ret = new HSBColor_Sapphir(0f, 0f, 0f, color.a);

        var r = color.r;
        var g = color.g;
        var b = color.b;

        var max = Mathf.Max(r, Mathf.Max(g, b));

        if (max <= 0)
        {
            return ret;
        }

        var min = Mathf.Min(r, Mathf.Min(g, b));
        var dif = max - min;

        if (max > min)
        {
            if (g == max)
            {
                ret.h = (b - r) / dif * 60f + 120f;
            }
            else if (b == max)
            {
                ret.h = (r - g) / dif * 60f + 240f;
            }
            else if (b > g)
            {
                ret.h = (g - b) / dif * 60f + 360f;
            }
            else
            {
                ret.h = (g - b) / dif * 60f;
            }

            if (ret.h < 0)
            {
                ret.h = ret.h + 360f;
            }
        }
        else
        {
            ret.h = 0;
        }

        ret.h *= 1f / 360f;
        ret.s = (dif / max) * 1f;
        ret.b = max;

        return ret;
    }

    public static Color ToColor(HSBColor_Sapphir HSBColor_Sapphir)
    {
        var r = HSBColor_Sapphir.b;
        var g = HSBColor_Sapphir.b;
        var b = HSBColor_Sapphir.b;
        if (HSBColor_Sapphir.s != 0)
        {
            var max = HSBColor_Sapphir.b;
            var dif = HSBColor_Sapphir.b * HSBColor_Sapphir.s;
            var min = HSBColor_Sapphir.b - dif;

            var h = HSBColor_Sapphir.h * 360f;


            r = min;
            g = min;
            b = h * dif / 360f + min;

        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), HSBColor_Sapphir.a);
    }

    public Color ToColor()
    {
        return ToColor(this);
    }

    public override string ToString()
    {
        return "H:" + h + " S:" + s + " B:" + b;
    }

    public static HSBColor_Sapphir Lerp(HSBColor_Sapphir a, HSBColor_Sapphir b, float t)
    {
        float h, s;

        //check special case black (color.b==0): interpolate neither hue nor saturation!
        //check special case grey (color.s==0): don't interpolate hue!
        if (a.b == 0)
        {
            h = b.h;
            s = b.s;
        }
        else if (b.b == 0)
        {
            h = a.h;
            s = a.s;
        }
        else
        {
            if (a.s == 0)
            {
                h = b.h;
            }
            else if (b.s == 0)
            {
                h = a.h;
            }
            else
            {
                var angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
                while (angle < 0f)
                    angle += 360f;
                while (angle > 360f)
                    angle -= 360f;
                h = angle / 360f;
            }

            s = Mathf.Lerp(a.s, b.s, t);
        }

        return new HSBColor_Sapphir(h, s, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
    }


}

public struct HSBColor_Quartz
{
    public float h;
    public float s;
    public float b;
    public float a;

    public HSBColor_Quartz(float h, float s, float b, float a)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        this.a = a;
    }

    public HSBColor_Quartz(float h, float s, float b)
    {
        this.h = h;
        this.s = s;
        this.b = b;
        a = 1f;
    }

    public HSBColor_Quartz(Color col)
    {
        var temp = FromColor(col);
        h = temp.h;
        s = temp.s;
        b = temp.b;
        a = temp.a;
    }

    public static HSBColor_Quartz FromColor(Color color)
    {
        var ret = new HSBColor_Quartz(0f, 0f, 0f, color.a);

        var r = color.r;
        var g = color.g;
        var b = color.b;

        var max = Mathf.Max(r, Mathf.Max(g, b));

        if (max <= 0)
        {
            return ret;
        }

        var min = Mathf.Min(r, Mathf.Min(g, b));
        var dif = max - min;

        if (max > min)
        {
            if (g == max)
            {
                ret.h = (b - r) / dif * 60f + 120f;
            }
            else if (b == max)
            {
                ret.h = (r - g) / dif * 60f + 240f;
            }
            else if (b > g)
            {
                ret.h = (g - b) / dif * 60f + 360f;
            }
            else
            {
                ret.h = (g - b) / dif * 60f;
            }

            if (ret.h < 0)
            {
                ret.h = ret.h + 360f;
            }
        }
        else
        {
            ret.h = 0;
        }

        ret.h *= 1f / 360f;
        ret.s = (dif / max) * 1f;
        ret.b = max;

        return ret;
    }

    public static Color ToColor(HSBColor_Quartz HSBColor_Quartz)
    {
        var r = HSBColor_Quartz.b;
        var g = HSBColor_Quartz.b;
        var b = HSBColor_Quartz.b;
        if (HSBColor_Quartz.s != 0)
        {
            var max = HSBColor_Quartz.b;
            var dif = HSBColor_Quartz.b * HSBColor_Quartz.s;
            var min = HSBColor_Quartz.b - dif;

            var h = HSBColor_Quartz.h * 360f;


            r = h * dif / 360f;
            g = min;
            b = max;

        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), HSBColor_Quartz.a);
    }

    public Color ToColor()
    {
        return ToColor(this);
    }

    public override string ToString()
    {
        return "H:" + h + " S:" + s + " B:" + b;
    }

    public static HSBColor_Quartz Lerp(HSBColor_Quartz a, HSBColor_Quartz b, float t)
    {
        float h, s;

        //check special case black (color.b==0): interpolate neither hue nor saturation!
        //check special case grey (color.s==0): don't interpolate hue!
        if (a.b == 0)
        {
            h = b.h;
            s = b.s;
        }
        else if (b.b == 0)
        {
            h = a.h;
            s = a.s;
        }
        else
        {
            if (a.s == 0)
            {
                h = b.h;
            }
            else if (b.s == 0)
            {
                h = a.h;
            }
            else
            {
                var angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
                while (angle < 0f)
                    angle += 360f;
                while (angle > 360f)
                    angle -= 360f;
                h = angle / 360f;
            }

            s = Mathf.Lerp(a.s, b.s, t);
        }

        return new HSBColor_Quartz(h, s, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
    }


}