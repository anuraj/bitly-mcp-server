# Bitly MCP Server
An MCP (Model Context Protocol) server implementation that integrates Claude with Bitly, enabling natural language short url management. This server allows Claude to interact with your Bitly links using everyday language.

## Supported features
* CreateBitlink
* UpdateBitlink
* RetrieveByBitlink
* GetLongUrlFromBitlink
* DeleteBitlink
* GetClickCountByMonth

## Using this in Claude Desktop

* Open the `claude_desktop_config.json` and add the following configuration.
```json
"bitly": {
    "command": "docker",
    "args": [
        "run",
        "-i",
        "--rm",
        "-e",
        "BITLY_API_KEY=YOUR_BITLY_API_KEY",
        "anuraj/bitlymcpserver"
    ],
    "env": {
        "BITLY_API_KEY": "YOUR_BITLY_API_KEY"
    }
}
```
* Restart the Claude desktop - in the MCP Tools you will get the bitly tools.
* Now you can write something like `Shorten this https://github.com/anuraj/bitly-mcp-server`.

## Disclaimer

This is an unofficial **MCP package for Bitly** and is not affiliated with, endorsed by, or maintained by Bitly.  
It is provided **for learning purposes only** and comes with **no guarantees or warranties of any kind**.  
Use at your own risk.


Happy Programming.