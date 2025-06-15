**MainDab ~~was~~ is a Roblox exploit that bypasses key systems implemented in Roblox exploit APIs.**

At the moment, MainDab solely utilises the [WeAreDevs API](https://wearedevs.net/d/Exploit%20API). 

**MainDab works as of June 2025**. 

Compared to what MainDab was in 2023, before a partial resurrection on June 2025, MainDab is essentially a more functional version of WRD's [JJSploit](https://wearedevs.net/d/JJSploit). 

[Script hub function in 2023](https://github.com/Avaluate/MainDab/assets/126605163/67fcf747-64da-429a-9e31-e0f783940095) (video)
## Download

## FAQ
### Is MainDab a virus?
No. MainDab is not obfuscated. This is explained in the "Risks of Using MainDab" section.
### Why was MainDab "resurrected"?
I have primarily moved onto doing Roblox development. I want to make sure my own Roblox projects are secure.

I did not want to be bothered with advertisements, so I resurrected MainDab.
### How is the key system bypassed?
It ultimately depends on which exploit API you use.
#### WeAreDevs
WeAreDevs key system uses (mboost.me)[https://mboost.me]. 

According to (mboost's API documentation)[https://docs.mboost.me/], WRD key system sends a POST request to mboost to see if a user has completed a task, with `{"success": true}` indicating the key system complete.

Thus, all that is required is to locally host a server (on localhost) that returns `{"success": true}`, alongside locally directing all traffic from mboost.me to localhost. 

This method requires a self-signed SSL certificate to be installed.

An alternative method would be to create a browser extension specific to mboost.me. However redirecting traffic locally avoids WRD opening your browser to mboost.me.


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
