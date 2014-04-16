 - A [*skiplist*](http://en.wikipedia.org/wiki/Skiplist) is a probabilistic datastructure that does largely the same job as a balanced binary search tree. 
 - A *cyclic skiplist* is a skiplist that links the tail pointers of each level to the head element of that level, forming loops.
 - A *deterministic skiplist* is a skiplist with the randomness removed as per [Thomas Papadakis's 1993 thesis](https://cs.uwaterloo.ca/research/tr/1993/28/root2side.pdf).

This project was an attempt at developing a natural, *O(lg n)*-time searchable representation of [cyclically-ordered](http://en.wikipedia.org/wiki/Cyclic_ordering) data by combining cyclic and deterministic skiplists.

The idea of this project emerged while I was working on [spherical Voronoi diagrams](https://github.com/andyljones/spherical-voronoi-diagram) as part of my shallow-fluid model project. One of the [papers](http://e-lc.org/tmp/Xiaoyu__Zheng_2011_12_05_14_35_11.pdf) I was following suggested a cyclic skiplist to represent the sweepline, but being uncomfortable with the randomness involved I wanted to use a determistic variant.

Unfortunately, this being one of my earliest programming experiences I made a complete hash of it. It is bloated, slow and prone to floating-point comparison errors. I eventually abandoned it after I found a conceptually simpler scheme to use for my shallow-water model, but still I hope to return and rewrite this in future.
