# MORS ARCIUM

A chaotic battle platformer where you play as one of four player classes in an
endless death arena. Every kill causes you to turn into the class of your victim.
Each wave of battles may be complicated by randomly chosen effects such as low gravity, sudden death, flooding, and visits from Satan.

This game was originally released in 2016, but I have just recently moved the
source repository from BitBucket to Github so I can have more of my major projects
on the same platform. Most of the code here was written at a time before I started
to really care about writing "clean" code, so my apologies for that.

The code is licensed under GPL3 (detailed in the LICENSE file) except when the copyright notice indicates otherwise, and art assets are under [Creative Commons Attribution Noncommercial](https://creativecommons.org/licenses/by-nc/3.0/). MorsArcium_Shared/Noise.cs is a simplex noise implementation released under public domain by Heikki Törmälä.

This game uses the Monogame framework for rendering, sound, and input. The folder "MorsArcium_Desktop" contains code specific for the Desktop platform, and the folder "MorsArcium_Shared" contains code for all platforms. They communicate using the IPlatformOutlet interface.

[The game may be downloaded and played from my website.](https://www.tophatdemon.com/en/games/mors-arcium)

There you will also find links to an Android version, although it is not up to date with this repository.

