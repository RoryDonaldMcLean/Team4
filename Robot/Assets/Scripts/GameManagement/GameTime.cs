public class GameTime  {
    
    private int hour;
    private int min;
    private int sec;

    public GameTime()
    {
        hour = 0;
        min = 0;
        sec = 0;
    }

    public GameTime(int sec)
    {
        hour = 0;
        min = 0;
        this.sec = sec % 60;
        min += sec / 60;
    }

    public GameTime(int min, int sec)
    {
        hour = 0;
        this.sec = sec % 60;
        this.min = (min + sec / 60) % 60;
        hour += (min + sec / 60) / 60;
    }

    public GameTime(int hour, int min, int sec)
    {
        this.sec = sec % 60;
        this.min = (min + sec / 60) % 60;
        this.hour = hour + (min + sec / 60) / 60;
    }

    public void SetGameTime(int sec)
    {
        this.sec = sec;
    }

    public void SetGameTime(int min, int sec)
    {
        this.min = min;
        this.sec = sec;
    }

    public void SetGameTime(int hour, int min, int sec)
    {
        this.hour = hour;
        this.min = min;
        this.sec = sec;
    }

    public int GetGameTimeHour()
    {
        return hour;
    }

    public int GetGameTimeMin()
    {
        return min;
    }

    public int GetGameTimeSec()
    {
        return sec;
    }

    public static GameTime operator +(GameTime x, GameTime y)
    {
        GameTime result = new GameTime();
        result.sec = x.sec + y.sec;
        result.min = result.sec >= 60 ? 1 : 0 + x.min + y.min;
        result.hour = result.min >= 60 ? 1 : 0 + x.hour + y.hour;
        result.sec = result.sec % 60;
        result.min = result.min % 60;
        return result;
    }

    //public static GameTime operator -(GameTime x, GameTime y)
    //{
    //    GameTime result = new GameTime();
    //    result.sec = x.sec - y.sec;
    //    result.min = x.min - y.min - result.sec < 0 ? 1 : 0;
    //    result.hour = x.hour - y.hour - result.min < 0 ? 1 : 0;
    //    result.sec = result.sec < 0 ? Mathf.Abs(result.sec) : result.sec;
    //    result.min = result.min < 0 ? Mathf.Abs(result.sec) : result.min;
    //    return result;
    //}

    public static bool operator >(GameTime x, GameTime y)
    {
        return x.hour * 3600 + x.min * 60 + x.sec > y.hour * 3600 + y.min * 60 + y.sec ? true : false; 
    }

    public static bool operator <(GameTime x, GameTime y)
    {
        return x.hour * 3600 + x.min * 60 + x.sec < y.hour * 3600 + y.min * 60 + y.sec ? true : false;
    }

    public static bool operator ==(GameTime x, GameTime y)
    {
        return x.hour * 3600 + x.min * 60 + x.sec == y.hour * 3600 + y.min * 60 + y.sec ? true : false;
    }

    public static bool operator !=(GameTime x, GameTime y)
    {
        return x.hour * 3600 + x.min * 60 + x.sec != y.hour * 3600 + y.min * 60 + y.sec ? true : false;
    }

    public static bool operator >=(GameTime x, GameTime y)
    {
        return x.hour * 3600 + x.min * 60 + x.sec >= y.hour * 3600 + y.min * 60 + y.sec ? true : false;
    }

    public static bool operator <=(GameTime x, GameTime y)
    {
        return x.hour * 3600 + x.min * 60 + x.sec <= y.hour * 3600 + y.min * 60 + y.sec ? true : false;
    }

    public bool Equals(GameTime other)
    {
        return this==other;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is GameTime ? Equals((GameTime)obj) : false;
    }

    public override int GetHashCode()
    {
        return 0;
    }
}

