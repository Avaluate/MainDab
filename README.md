# MainDab
MainDab ~~was~~ is a Roblox exploit that solely utilises the [WeAreDevs API](https://wearedevs.net/d/Exploit%20API). Compared to what was in 2023, MainDab is essentially a more functional version of WRD's [JJSploit](https://wearedevs.net/d/JJSploit). **MainDab works as of June 2025**.

[MainDab's Game Hub in operation in 2023](https://github.com/Avaluate/MainDab/assets/126605163/67fcf747-64da-429a-9e31-e0f783940095) (video)
## Download

## FAQ
### Won't using Roblox exploits get you banned nowadays?
The simple answer: yes.
The exception: **exploiting in your __own__ Roblox game is allowed.**

[From Roblox, 30 May 2025](https://devforum.roblox.com/t/an-update-on-automated-action-against-modified-clients/3640609):
> *As of today, __anyone with edit permissions for a place will not be actioned against if we detect them using a modified client in that place only__. In other words, if you’re a creator and want to test your anti-cheat system against an exploit, you may do so.*
### Is MainDab a virus?
No. You can decompile MainDab using a .NET decompiler like [ILSpy](github.com/icsharpcode/ILSpy). WeAreDevs API, as of time of writing, is not malware. 

**However, WeAreDevs API has an auto update function**. This implies malware can be delivered to you without your knowledge. This, however, has never occured. If you are afraid of a virus, run MainDab and Roblox in a Virtual Machine. [Just be sure to use GPU passthrough](https://clayfreeman.github.io/gpu-passthrough/).

Ironically, MainDab also has an auto-update function for convenience. You can remove this auto-update functionality if you don't trust me by compiling MainDab yourself.
### Wasn't MainDab discontinued?
MainDab was discontinued around mid-2023 due to both more stingent anti-exploit measures and a lack of interest from my side. 

**However, MainDab has been partially resurrected and still works as of June 2025, as there was a need for me to secure my own Roblox game projects.**
### How did you implement checking WeAreDevs status? That's not offered in the library
a bit of reverse engineering (i know none, but with some common sense, some research and 2 hours spent i figured it out)
technically speaking the API does check the status but doesn't export functions to check it
### Why is the code so shit?
Because 3 years ago I did not know how to organise my code properly, and also because most of this is code last touched in late 2022.
### Can I use MainDab's source code?
I have not added any licensing to MainDab. MainDab uses other components with their own licenses. I will have to check every single library used to determine whether or not you one is free to use MainDab.

My own guess is that you can use MainDab's source code for non-commercial purposes. In order words, you must not attach a "key system" to modified version.
