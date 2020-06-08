# NDimArray
Provides a generic wrapper around the Array object, along with indexed enumeration.

See ![the wiki](https://github.com/SpencerAWill/NDimArray/wiki) for (**unfinished**) documentation.

---
**I have yet to push unit tests so take this project with a grain of salt.**
---

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

This would otherwise be known as:
```C#
var array = new int[,,]
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
Will show:
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
//These parameters define a REVERSE enumeration through this array.

array.Enumerate(
  new int[] { 1, 1, 1 }, //index of the last item [1,1,1]
  new int[] { 0, 0, 0 }, //index of the first item [0,0,0]
  new int[] { 2, 1, 0 }, //default depth-column-row enumeration (feel free to experiment with distinct priority lists
  (index, item) => { Console.WriteLine($"[{String.Join(", ", index)}]: { item }"); } //action on each item
  );
```
Will show:
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

### Enumeration excluding a dimension
```C#
//With point enumeration, you can exclude a dimension from enumeration. I.e. enumerate through a 2D plane in a 3D array

array.Enumerate(
  new int[] { 0, 0, 0 },
  new int[] { 1, 0, 1 },
  new int[] { 2, 1, 0 }, //in this case, because we are not going to moving through dimension 1, this functions more like a { 2, 0 } priority list)
  (index, item) => { Console.WriteLine($"[{String.Join(", ", index)}]: { item }"); } //action on each item
);
```
Will show:
```C#
[0, 0, 0]: "a"
[0, 0, 1]: "b"
[1, 0, 0]: "e"
[1, 0, 1]: "f"
```
