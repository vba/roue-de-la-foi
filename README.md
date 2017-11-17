# roue-de-la-foi

### Task Description 

![](https://habrastorage.org/webt/sl/yh/bs/slyhbsgur7mmhy4f01j2f0zooze.png)

### Implementation

The implementation of business rules is contained in `SupportWheelOfFate.Core` module. I found this task interesting and I have identified and coded the solution not only for _10 engineers_ but for 4 .. `Int32.MaxValue` potential engineers. I have used very banal approach to solve this problem. I took in consideration two eventual cases where the count of involved engineers can be an odd or an even number.     

#### Even case scenario

It's obviously the simplest scenario. We need to take a random sequence of involved engineers, of length `n` and create a reversed copy of such sequence. Then we can zip them and obtain following result:

```
n = 6
[4; 2; 5; 3; 1; 6]
[6; 1; 3; 5; 2; 4]

The problem is situated in the middle:
[.. 5; 3; ..]
[.. 3; 5; ..]

In order to solve it we just need to bring one slice from the middle to the beginning of zipped sequence

[4; 2; 5; 3; 1; 6]  =>  [3; 4; 2; 5; 1; 6]
[6; 1; 3; 5; 2; 4]  =>  [5; 6; 1; 3; 2; 4]

``` 

#### Odd case scenario

Here the preliminary steps (revers and zip) are the same. But the solution would be following:

```
n = 5
[4; 2; 5; 3; 1]
[1; 3; 5; 2; 4]

The problem is again situated in the middle, but has a different nature comparing to the Even Case:
[.. 2; 5; 3; ..]
[.. 3; 5; 2; ..]

This problem can be solved in 2 following steps

[4; 2; 5; 3; 1]  =>  [5; 2; 4; 3; 1]  =>  [5; 2; 4; 3; 1]
[1; 3; 5; 2; 4]  =>  [1; 3; 5; 2; 4]  =>  [3; 1; 5; 2; 4]

```

#### Data provided by default

I've used a custom people generator, which can combine up to 97 random First Name / Last Names tuples. But normally it should work with bigger amount of data. No rules are hardcoded in the core part, because my assumption is proven on the tests side. For this sensible part I've used a property based testing approach.

#### Complexity

Time complexity of my solution is O(n), same is the memory complexity. It could be possible to improve time and memory complexity by using mutable structures in combination with custom `swap` function. Finite state machine complexity can be reduced with the usage of transducers.

### How to run

Server part can be run simply from Visual Studio or can be compiled and run as a simple console application. For the purpose of simplicity, the port is hardcoded to `5000`. Once run you can try out a simple test request to `http://localhost:5000/api/wheel/10`.

The client part was implemented with `Angular 4`, hence in order to run it, you need to have `node` and `angular` package installed on your machine. Once installed you will need to:
```
cd SupportWheelOfFate.Client/roue-du-destin
npm install
ng serve
```

Then just go to `http://localhost:9090` and you should see the simplistic interface.