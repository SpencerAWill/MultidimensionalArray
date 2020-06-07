# NDimArray
Provides a generic wrapper around the Array object, along with indexed enumeration.

See ![the wiki](https://github.com/SpencerAWill/NDimArray/wiki/SpatialEnumerator) for documentation.

### Creating a new 2×2×2 array of strings:
```C#
var array = new NDimArray<string>(2, 2, 2);

array[0,0,0] = "a";
array[0,0,1] = "b";
array[0,1,0] = "c";
array[0,1,1] = "d";
array[1,0,0] = "e";
array[1,0,1] = "f";
array[1,1,0] = "g";
array[1,1,1] = "h";
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
//These parameters define a backwards enumeration through this array.

array.EnumerateCustom(
  SpatialEnumerator<string>.UpperBoundaries(array), //index of the last item [1,1,1]
  SpatialEnumerator<string>.LowerBoundaries(array), //index of the first item [1,1,1]
  SpatialEnumerator<string>.GetStandardPriorityList(array.Rank), //default depth-column-row enumeration (feel free to change the ordering of these elements around)
  (index, item) => Console.WriteLine($[{index.Join(", ")]}: {item}) //action on each item
  );
```
Will return:
```C#
[1, 1, 1]: h
[1, 1, 0]: g
[1, 0, 1]: f
[1, 0, 0]: e
[0, 1, 1]: d
[0, 1, 0]: c
[0, 0, 1]: b
[0, 0, 0]: a
```
