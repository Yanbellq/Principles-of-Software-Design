// Console.WriteLine("Hello, World!");

string? x = null;

string a = "a";
string b = "b";

var compareTo = a.CompareTo(b);
var str = String.Concat(a, b);
var contains = a.Contains(b);
var joined = String.Join("qwe", new string[] { "a", "b", "c", "d" });
// string? joined = "";
// String.CompareOrdinal();
// string? va = null;
// char[] destination = new char[100];
// joined.CopyTo(new char[] {});
// joined.CopyTo(destination);
// Console.WriteLine(destination);

var startWith = joined.IndexOf("qwe");
var insert = joined.Insert(3, "Maksym");
Console.WriteLine(insert);

var replace = insert.Replace("Rostyk", "");
Console.WriteLine(replace);

var strings = replace.Split(a, b);


var substring = "hello world".Substring(3, 6);
Console.WriteLine(substring.ToUpper());
