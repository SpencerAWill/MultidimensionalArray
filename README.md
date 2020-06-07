# NDimArray
Provides a generic wrapper around the Array object, along with indexed enumeration.

See ![the wiki](https://github.com/SpencerAWill/NDimArray/wiki/SpatialEnumerator) for documentation.

### Creates a new 2×2×2 array of strings:
```C#
var array = new NDimArray<string>(2, 2, 2);

then assume tedious assignment of letters.
```
This is internally:
```C#
{
  {
    { "a", "b" },
    { "c", "d" }
  },
  {
    { "e", "f" },
    { "g", "h" }
  }
}
```

### Regular enumeration through this array:
```C#
array.Enumerate(x => Console.WriteLine(x));
```
Will return:
```C#
a
b
c
d
e
f
g
h
```

### Advanced enumeration through this array
```C#
array.EnumerateCustom(
  SpatialEnumerator<string>.UpperBoundaries(array),
  SpatialEnumerator<string>.LowerBoundaries(array),
  SpatialEnumerator<string>.GetStandardPriorityList(array.Rank),
  (index, item) => Console.WriteLine($[{index.Join(", ")]}: {item})
  );
```
Will return:
```C#
[0, 0, 0]: a
[0, 0, 1]: b
[0, 1, 0]: c
[0, 1, 1]: d
[1, 0, 0]: e
[1, 0, 1]: f
[1, 1, 0]: g
[1, 1, 1]: h
```
