namespace AspNetCoreAwsServerless.Utils.Id;

/// <summary>
/// Strongly typed wrapper for Guids. Makes it harder to accidentally mix up, for example, User Ids and Book Ids.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct Id<T>
{
  private readonly Guid _value;

  public Id()
  {
    _value = Guid.NewGuid();
  }

  public Id(string value)
  {
    Guid val = ParseValue(value);
    _value = val;
  }

  public Id(Guid value)
  {
    CheckValue(value);
    _value = value;
  }

  private static Guid ParseValue(string value)
  {
    bool wasParsed = Guid.TryParse(value, out Guid guid);
    if (!wasParsed)
      throw new ArgumentException("Invalid Guid value", value);
    CheckValue(guid);
    return guid;
  }

  private static void CheckValue(Guid value)
  {
    if (value == Guid.Empty)
      throw new ArgumentException("Guid value cannot be empty", nameof(value));
  }

  public override string ToString()
  {
    return _value.ToString();
  }

  public static implicit operator Id<T>(string s) => new(s);

  public static implicit operator Id<T>(Guid id) => new(id);
}
