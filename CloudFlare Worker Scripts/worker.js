async function createJWT(username) {
    const header = {alg: "HS256", typ: "JWT"};
    const encodedHeader = btoa(JSON.stringify(header));
  
    const payload = {
      sub: username,
      iat: Date.now() / 1000,
      exp: Date.now() / 1000 + 604800, //Token expires after 7 days
    };
  
    const encodedPayload = btoa(JSON.stringify(payload));
  
    const signature = await createSignature(encodedHeader + "." + encodedPayload, globalThis.JWT_SECRET);
    return encodedHeader + "." + encodedPayload + "." + signature;
  
  
  }
  
  function base64ToArrayBuffer(base64) {
    var binary_string = atob(base64);
    var len = binary_string.length;
    var bytes = new Uint8Array(len);
    for (var i = 0; i < len; i++) {
        bytes[i] = binary_string.charCodeAt(i);
    }
    return bytes.buffer;
  }
  
    async function createSignature(message, secret) {
    const encoder = new TextEncoder();
    const data = encoder.encode(message);
    const keyData = base64ToArrayBuffer(secret);
    //Import key
    const key = await crypto.subtle.importKey(
      'raw',
      keyData,
      { name: 'HMAC', hash: { name: 'SHA-256' }},
      false,
      ['sign']
    );
    // Sign the data
    const signature = await crypto.subtle.sign(
      'HMAC',
      key,
      data
    );
    //Convert signature to Base64 string
    return btoa(String.fromCharCode(...new Uint8Array(signature)));
  
    }
  
  
  
  addEventListener('fetch', event => {
    event.respondWith(handleRequest(event.request))
  })
  
  async function handleRequest(request) {
    if (request.method === "GET" && request.url.endsWith("/login")) {
      const usernamePasswordBase64 = request.headers.get("Authorization");
      const username = request.headers.get("username");
      const apiKey = globalThis.API_KEY;
  
      const authResponse = await fetch("https://home.myopenhab.org/rest/things/zwave%3Adevice%3Ae804a908f8%3Anode14", {
        headers: {
          "Authorization": usernamePasswordBase64,
          "Cookie" : "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true",
          "X-OPENHAB-TOKEN": apiKey
          
        }
      });
  
  
      
      if (authResponse.ok) {
        // If the request is successful, return a positive response
        const jwt = await createJWT(username);
        return new Response(JSON.stringify({token: jwt}), {
          headers: {'Content-Type': 'application/json'},
          status: 200
        });
      } else {
        // Handle failed authentication
        console.log("Cloudworker failed request to openhab");
        return new Response("Authentication Failed", { status: authResponse.status });
      }
    }
  
    // If the url ends with validate encode the token and return a response if it is valid(The token is only valid for 7 days)
  
    if (request.method === "GET" && request.url.endsWith("/validate")) {
        const token = request.headers.get("Authorization")?.replace("Bearer ", "");
        const parts = token.split(".");
  
        const payload = JSON.parse(atob(parts[1].replace(/-/g, '+').replace(/_/g, '/')));
    
        // Check if the token is expired
        if (payload.exp && Date.now() >= payload.exp * 1000) {
        return new Response("Expired Token", { status: 401 });
        }
  
  
        const signature = await createSignature(parts[0] + "." + parts[1], globalThis.JWT_SECRET);
        if (signature === parts[2]) {
            return new Response("Valid Token", { status: 200 });
        } else {
            return new Response("Invalid Token", { status: 401 });
        }
    }
  
    if (request.method === "GET" && request.url.endsWith("/retrieveinfo")) {
      const usernamePasswordBase64 = globalThis.ADMIN_BASE;
      const uid = request.headers.get("UID");
      const apiKey = globalThis.API_KEY;
      const apiLink = `https://home.myopenhab.org/rest/things/${uid}`;
  
      console.log("ApiLink:", apiLink);
  
  
      try {
        const authResponse = await fetch(apiLink, {
          headers: {
            "Authorization": `Basic ${usernamePasswordBase64}`,
            "Cookie" : "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true",
            "X-OPENHAB-TOKEN": apiKey
          }
        });
  
        if (authResponse.ok) {
          const data = await authResponse.json();
          return new Response(JSON.stringify(data), {
            headers: {'Content-Type': 'application/json'},
            status: 200
        });
  
        } else {
  
          console.error('HTTP Error:', authResponse.statusText);
          return new Response(`Error: ${authResponse.statusText}`, { status: authResponse.status });
  
        }
  
  
      } catch (error) {
  
        console.error('Fetch Error:', error);
        return new Response(`Error: ${error.message}`, { status: 500 });
      }
      
  
    }
  
    if (request.method === "GET" && request.url.endsWith("/sendcommand")) {
      const commandType = request.headers.get('command-type');
      const commandValue = request.headers.get('command-value');
  
      if(commandType === 'color') {
        const currentState = await getLampState();
        if(currentState !== 'ON') {
          await sendCommand('https://home.myopenhab.org/rest/items/Ceiling_Lamp_switch_21_01', 'ON');
          await new Promise(resolve => setTimeout(resolve, 1000)); // Wait for 1 second
        }
        return sendCommand('https://home.myopenhab.org/rest/items/Ceiling_Lamp_color_21_03', commandValue);
      } else {
  
        const commandUrl = 'https://home.myopenhab.org/rest/items/Ceiling_Lamp_switch_21_01' 
          return sendCommand(commandUrl, commandType.toUpperCase());
      }
    if (request.method === "GET" && request.url.endsWith("/getitemstate")) {
    const linkedItemName = request.headers.get('item-name');
    const stateUrl = `https://home.myopenhab.org/rest/items/${linkedItemName}/state`;
    const headers = {
        'Authorization': `Basic ${globalThis.ADMIN_BASE}`,
        "Cookie": "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true",
        "X-OPENHAB-TOKEN": globalThis.API_KEY
    };

    const response = await fetch(stateUrl, { headers });
    if (!response.ok) {
        throw new Error(`Failed to get item state: ${response.statusText}`);
    }
    
    // Extract the text from the response
    const stateText = await response.text();

    return new Response(JSON.stringify({ state: stateText }), {
        headers: { 'Content-Type': 'application/json' },
        status: 200
    });
  
      
  
  
    }
  
  
    return new Response("This worker only handles specific requests", { status: 400 });
  }
  
  async function getLampState() {
    const stateUrl = 'https://home.myopenhab.org/rest/items/Ceiling_Lamp_switch_21_01/state';
    const headers = {
      'Authorization': `Basic ${globalThis.ADMIN_BASE}`,
      "Cookie": "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true",
      "X-OPENHAB-TOKEN": globalThis.API_KEY
    };
  
    const response = await fetch(stateUrl, { headers });
    if (!response.ok) {
      throw new Error(`Failed to get lamp state: ${response.statusText}`);
    }
    return response.text();
  }
  
  async function sendCommand(url, command) {
    const headers = {
      'Authorization': `Basic ${globalThis.ADMIN_BASE}`,
      "Cookie" : "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true",
      "X-OPENHAB-TOKEN": globalThis.API_KEY,
      'Content-Type': 'text/plain'
    };
  
    const response = await fetch(url, {
      method: 'POST',
      headers: headers,
      body: command
    });
  
    return new Response(`Command sent: ${command}`, { status: response.status });
  
  }
