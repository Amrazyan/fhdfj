using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CustomExpandingMethods
{

    public enum Colors
    {
        yellow,
        red,
        purple,
        green
    }

    public static bool IsValidMail(string emailAddress)
    {
        return Regex.IsMatch(emailAddress, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, @"^[0-9]+$");
    }

    public static string FormatNumber(long num)
    {
        long i = (long)System.Math.Pow(10, (int)System.Math.Max(0, System.Math.Log10(num) - 2));
        num = num / i * i;

        if (num >= 1000000000)
            return (num / 1000000000D).ToString("0.##") + "B";
        if (num >= 1000000)
            return (num / 1000000D).ToString("0.##") + "M";
        if (num >= 1000)
            return (num / 1000D).ToString("0.##") + "K";

        return num.ToString("#,0");
    }
    public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
    {
        if (rectTransform == null) return;

        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = rectTransform.pivot - pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
    }

    public static string sha256_hash(string value)
    {
        StringBuilder Sb = new StringBuilder();

        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
    
    public static string GetTimeMinute(this string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return (Convert.ToInt16(value) / 60) + "m";
    }

    public static string Color(this string value, Colors color)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return $"<color={color.ToString()}>{value}</color>";
    }
    public static string CurrentTimeStamp(this System.DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssffff");
    }

    static string hms = @"hh\:mm\:ss";
    static string dhms = @"dd\hh\:mm\:ss";

    public static string GetFormatedTime(this System.TimeSpan value)
    {
        //string typeOfRetutn = "";
        //if (value.Days > 0)
        //{
        //    value.we
        //}
        return value.ToString(hms);
    }

    public static string DateTimeGoodFormat(this System.DateTime value)
    {
        return string.Format("{0:MMM d, hh:mm}", value);
    }

    public static string GetHourMinute(this System.DateTime value)
    {
        return value.Hour + " : " + value.Minute.ToString("00");
    }

    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
    public static void SetWidth(this RectTransform rt, float width)
    {
        rt.sizeDelta = new Vector2(width,rt.sizeDelta.y);
    }
    public static void SetHeight(this RectTransform rt, float height)
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }

    public static float InitSliderValue01(float currentValue, float maxValue)
    {
        return currentValue / maxValue;
    }


    public static float fromAnyValueTo01(float currentValue, float maxValue, float minValue = 0)
    {
        if (currentValue - minValue == 0)
        {
            return 0;
        }
        if (maxValue - minValue == 0)
        {
            return currentValue - minValue;
        }
        return (currentValue - minValue) / (maxValue - minValue);
    }

    public static float fromAnyValueTo01(double currentValue, double maxValue, double minValue = 0)
    {
        return (float)((currentValue - minValue) / (maxValue - minValue));
    }

    public static float from01ToActualValue(float currentValue, float targetValue, float minValue = 0)
    {
        return minValue + ((targetValue - minValue) * currentValue);
    }

    public static double from01ToActualValue(float currentValue, double maxValue, double minValue = 0)
    {
        return minValue + ((maxValue - minValue) * currentValue);
    }

    public static float fromOneValueToAnother(float oneCurrentValue, float oneMaxValue, float targetValue, float oneMinValue = 0, float returnValue = 0)
    {
        float zeroToOne = fromAnyValueTo01(oneCurrentValue, oneMaxValue, oneMinValue);

        return from01ToActualValue(zeroToOne, targetValue, returnValue);

    }

}
