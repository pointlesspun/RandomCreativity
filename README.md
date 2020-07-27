
<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/e/ea/Van_Gogh_-_Starry_Night_-_Google_Art_Project.jpg/757px-Van_Gogh_-_Starry_Night_-_Google_Art_Project.jpg" style="object-fit:cover" width="100%" height="256px"/>

Random Creativity
=================

Is it possible to create truly creative, suprising procedural content? Or will the end result always be a predictable product of the list of the parameters of procedural heuristic? This project is a modest investigation of this question. 

The intention is to have some basic maze generation code in place and try to expand with variations around this concept. What happens if the shape of the maze gets varied? The colors? Does this result in  interesting mazes or more just more randomizations without soul?

To be clear, I'm not expecting the level of creativity that will result in the heuristic coming up with a lake with a temple in the middle and a [giant snake surrounding its mountain ranges](https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.reddit.com%2Fr%2FVirtualPhotographers%2Fcomments%2Feomtwj%2Fgod_of_war_4_and_the_world_serpent%2F&psig=AOvVaw0K3YTu3Xn79ltR_lwtD6-J&ust=1595930479359000&source=images&cd=vfe&ved=0CAIQjRxqFwoTCLiMucCW7eoCFQAAAAAdAAAAABAD). I'm more aiming to something that is a notch above the run-of-the-mill-rogue-like cookie cutter template of mazes.

The implementation is done in [Unity](https://unity.com/), the game engine of choice but (hopefully) most of the implementation that is not Unity specific can be done engine-independent. Note that _all demos below can only be run from a webserver_.  

The following logs capture this effort:

1. [First steps](Doc/first-steps.md), drawing a simple triangle in Unity.
2. [Live updating the polygons](Doc/updating-the-editor.md), how to get the triangles to show up in the editor view of Unity ([Demo](Doc/Html/TwoTriangles/index.html)). 
