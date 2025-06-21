<p align="center">
  <a href="https://maindab.org/discord">
    <img src="https://raw.githubusercontent.com/Avaluate/MainDab/refs/heads/main/Cover/MainDabThumbnail.png" alt="Logo" width=70% height=70%>
  </a>
</p>
<p align="center">
    <a title="Download MainDab" href="https://github.com/Avaluate/MainDab/releases"><img alt="Download MainDab" src="https://raw.githubusercontent.com/Avaluate/MainDab/refs/heads/main/Cover/Download.png" width=200 height=53></a>
    <a title="Instructions" href="https://maindab.gitbook.io/maindabdocs"><img alt="Insructions" src="https://raw.githubusercontent.com/Avaluate/MainDab/refs/heads/main/Cover/Instructions.png" width=200 height=53></a>
    <a title="Discord" href="https://maindab.org/discord"><img alt="Discord" src="https://raw.githubusercontent.com/Avaluate/MainDab/refs/heads/main/Cover/Discord.png" width=200 height=53></a>
    <a title="Telegram" href="https://t.me/maindabnow"><img alt="Telegram" src="https://raw.githubusercontent.com/Avaluate/MainDab/refs/heads/main/Cover/Telegram.png" width=200 height=53></a>
  </p>

**MainDab is a Roblox exploit that bypasses key systems implemented in Roblox exploit APIs. MainDab works as of June 2025.** 

At the moment, MainDab solely utilises the [WeAreDevs API](https://wearedevs.net/d/Exploit%20API). As such, MainDab is essentially a more functional version of WRD's [JJSploit](https://wearedevs.net/d/JJSploit).

MainDab was previously discontinued on April 2023, before a partial resurrection on June 2025. 

Changelog: see [Changelog.txt](https://raw.githubusercontent.com/Avaluate/MainDab/refs/heads/main/Changelog.txt).
## Download
Go to [releases](https://github.com/Avaluate/MainDab/releases) or compile MainDab yourself. Please [read the instructions](https://maindab.gitbook.io/maindabdocs) first.
## Is MainDab a virus?
No. MainDab is not obfuscated either; you are free to look at the (shitty) code.

This is explained further in the [help website](https://maindab.gitbook.io/maindabdocs).
## Can you get banned for Roblox for using exploits?
Yes, but Roblox allows you to exploit [only in your own games](https://devforum.roblox.com/t/an-update-on-automated-action-against-modified-clients/3640609).
## Features
- The key system bypass is what makes MainDab different from other exploits. This process is fully automated
- MainDab also has standard features found in other exploits:
  - A status page showing which APIs are patched by Roblox
  - A nice text editor with syntax highlighting and multiple tabs - MainDab has its [own AvalonEdit definition](https://github.com/Avaluate/MainDabWeb/blob/main/Themes/lua_md_default.xshd) specifically for Roblox Lua
  - A [general scripts hub](https://github.com/Avaluate/MainDabWeb/blob/main/UpdateStuff/Scripts.json) and a [game hub](https://github.com/Avaluate/MainDabWeb/blob/main/UpdateStuff/GameHubScripts.json)
  - [Custom themes](https://github.com/Avaluate/MainDabWeb/tree/main/UIThemes) as seen in the cover image
  - Some other tools (i.e. [Multiple Roblox Instances](https://github.com/Avaluate/MultipleRobloxInstances))
## Help/FAQ
See the [help website](https://maindab.gitbook.io/maindabdocs).
## Support/Questions
- Discord: avaluate
- Telegram: t.me/avaluate
- Email: avaluate@maindab.org


### Is MainDab a virus?
No. MainDab is not obfuscated. This is explained in the "Risks of Using MainDab" section. 
### Why was MainDab "resurrected"?
I have primarily moved onto doing Roblox development. I want to make sure my own Roblox projects are secure.

I did not want to be bothered with advertisements, so I resurrected MainDab.
### Why is the key system bypassed?
Roblox exploit developers require funding to make their work worthwhile to continue and I recognise this need.

However:
* MainDab has long been disassociated with the exploiting community.
* The original objective of MainDab was to provide a Roblox exploit without adverts. This objective is still to be adhered to
* All Roblox exploits (including MainDab) go against Roblox Terms of Service and hence lack legitimacy.

I can remove the key system bypass on request.
### How is the key system bypassed?
It ultimately depends on which exploit API you use. The goal is to fake web requests stating one has completed a key system.
#### WeAreDevs
WeAreDevs key system uses (mboost.me)[https://mboost.me]. This is obvious both from using WRD's API itself and from disassembling the API.

According to (mboost's API documentation)[https://docs.mboost.me/], WRD key system sends a POST request to mboost to see if a user has completed a task, with `{"success": true}` indicating the key system complete.

Thus, all that is required is to locally host a server (on localhost) that returns `{"success": true}`, alongside locally directing all traffic from mboost.me to localhost. 

This method requires a self-signed SSL certificate to be installed.

An alternative method would be to create a browser extension specific to mboost.me. However, redirecting traffic locally avoids WRD opening your browser to mboost.me.
### Why is the code so shit?
Because 3 years ago I did not know how to organise my code properly, and also because most of this is code last touched in late 2022.
### Can I use MainDab's source code?
You can probably use MainDab's source code even for commercial purposes. All libraries used in MainDab use the MIT license.
### Wasn't MainDab discontinued?
MainDab was discontinued around mid-2023 due to both more stingent anti-exploit measures and a lack of interest from my side. 

However, MainDab has been partially resurrected and still works as of June 2025, as there was a need for me to secure my own Roblox game projects.
### How did you implement checking WeAreDevs status? That's not offered in the library
a bit of reverse engineering (i know none, but with some common sense, some research and 2 hours spent i figured it out)
technically speaking the API does check the status but doesn't export functions to check it

## The risks of using MainDab
There are two risks associated with using any Roblox exploit:
* If you are not using such exploits on your own games, a ban from Roblox
* Malware

MainDab does not address either of these issues. MainDab is merely a convenient frontend for using Roblox exploits.
### Bans from Roblox 
Simply put: Roblox has always had the ability to detect Roblox exploits. 

You risk a 1-day ban for initial rule breaks relating to exploiting. Further rule breaks could lead to harsher punishments, which could affect other Roblox accounts.

The scope of punishment in regards to exploiting is not clear.
### Malware from MainDab
MainDab can technically deliver malware to your computer though delivery from Roblox exploit APIs themselves (which MainDab cannot control).

MainDab, as a frontend for Roblox exploits, is not malware.

Unlike many other Roblox exploits frontend that also utilise Roblox exploit APIs:
* MainDab does not contain any advertisement or key system
* MainDab's code is not obfuscated. You are welcome to use a .NET decompiler like ILSpy to verify and see the code for yourself
* MainDab's code is also publicly available

In other words, MainDab is a transparent applicantion, but the Roblox exploit APIs used are not. 

Admittedly, MainDab has an auto-update function which could be seen as a security risk. This implies that if this GitHub account hosting MainDab were to be hijacked, a malicious actor could release malicious code to your computer.

By using MainDab, you trust in my efforts to keep my account secure.

Alternatively, you may compile MainDab yourself instead of using the binaries supplied in this repository.
### Malware from exploit APIs: WeAreDevs
WeAreDevs API also has an auto update function which MainDab cannot control (actually this is technically possible, but would add additional inconvenience to the end user).

This means the developers at WeAreDevs can deliver malware to you if they wish to.

However, since WeAreDevs creation (around ~2018?), there has not been a single case of malware released in WeAreDevs API. 
