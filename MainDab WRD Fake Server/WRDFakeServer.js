const https = require('https');
const fs = require('fs');

const options = {
    key: fs.readFileSync('mboost.me.key'),
    cert: fs.readFileSync('mboost.me.crt')
};

let server = null

try{
    server = https.createServer(options, (req, res) => {
    console.log(`Request received: ${req.method} ${req.url}`);

    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify(
        {
            success: true
       }
  ));   
});
}
catch(exception){
    console.log(`There was an error attempting to start the WRD API bypass: ${exception}`)
}

try{
    server.listen(443, '127.0.0.1', () => {
        console.log('MainDab WeAreDevs API Bypass\nHTTPS server running on https://127.0.0.1:443 and listening for requests from WRD');
    });
}
catch(exception){
    console.log(`There was an error attempting to start the WRD API bypass: ${exception}`)
}

process.on('uncaughtException', function (exception) {
  console.log(`There was an error attempting to start the WRD API bypass: ${exception}`);
});