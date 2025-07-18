# Bitly MCP Server
This MCP server will help integrate Claude with Bitly for natural language short URL management.

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

### MCPHub Certification
Bitly MCP Server is certified by [https://mcphub.com/](https://mcphub.com/). This certification ensures that `Bitly MCP Server` follows best practices for Model Context Protocol implementation.

## Disclaimer

This is an unofficial **MCP package for Bitly** and is not affiliated with, endorsed by, or maintained by Bitly.  
It is provided **for learning purposes only** and comes with **no guarantees or warranties of any kind**.  
Use at your own risk.


Happy Programming.
