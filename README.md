# NDimArray
Provides a generic wrapper around the Array object, along with indexed enumeration.

[Wiki/Documentation](https://github.com/SpencerAWill/NDimArray/wiki)


Find the latest NuGet package [here](https://github.com/SpencerAWill/NDimArray/packages)


:heavy_exclamation_mark:  [Issue template](https://github.com/SpencerAWill/NDimArray/tree/master/.github/ISSUE_TEMPLATE)  :heavy_exclamation_mark:

## Installation

### CLI
Install the latest package from the commandline using the following command:<br>
`dotnet add PROJECT package NDimArray`

### NuGet.exe CLI
Install the latest package from the nuget CLI using the following command:<br>
`nuget install NDimArray`

### Package Manager Console
Install the latest package from the Package Manager Console using the following command:<br>
`Install-Package NDimArray -ProjectName PROJECT`

## Examples

#### Creating a new 2×2×2 array of strings:
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

#### Regular enumeration through this array:
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


### Advanced enumeration

Using the overloaded `Enumerate` function, it is possible to enumerate between any 2 indices in the array.
The following are some examples of this:

#### Reverse Enumeration
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

#### Enumeration excluding a dimension (eg. 2D enumeration in a 3D array, 1D enumeration in a 2D array)
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

#### Enumeration excluding ANY number of dimensions (eg. 1D enumeration in a 3D array)
```C#
//With point enumeration, you can exclude a dimension from enumeration. I.e. enumerate through a 1D segment in a 3D array

array.Enumerate(
  new int[] { 0, 0, 0 }, 
  new int[] { 1, 0, 0 }, //notice the only dimension that will change is the 0th dimension
  new int[] { 2, 1, 0 }, //this list now only functions like a single element { 0 } because the 1st and 2nd dimension are excluded. 
  (index, item) => { Console.WriteLine($"[{String.Join(", ", index)}]: { item }"); } //action on each item
);
```
Will show:
```C#
[0, 0, 0]: "a"
[1, 0, 0]: "e"
```

#### Enumeration between any 2 points
```C#
var array = new NDimArray<string>(3, 3, 3); 

//assume we now fill all 27 elements a -> z then '!' as the 27th element

var start = new int[] { 0, 1, 1 }; //this puts us in the central element of the top of the cube
var end = new int[] { 2, 2, 0 }; //end will be the bottom right of the front of the cube
var priorities = new int[] { 1, 2, 0 }; //each element must be unique

//It is beneficial to visualize this array as a cube of side length 3.
//It is also beneficial to visualize the origin  [0, 0, 0] of the cube as the front top left of the cube, 
//  with dimension 0 extending down, dimension 1 extending right, and dimension 2 extending into the page.
//With higher dimensions (n>3), a visual representation will be difficult to grasp.
/*
 *         2
 *        /
 *       /--> o
 *      /    ^
 *     .___ _/_ ___ 1
 *     |         |
 *     |         V
 *     | ------> x
 *     0
 *
 * where 'o' is start
 * and 'x' is end
*/

array.Enumerate(
  start,
  end,
  priorities,
  x => Console.WriteLine(x)
  );
```

Will show:
```C#
e
h
d
g
n
q
m
p
w
z
v
y
