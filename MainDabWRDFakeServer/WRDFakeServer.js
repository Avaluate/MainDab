const https = require('https');
const fs = require('fs');
const readline = require('readline');

const options = {
    key: fs.readFileSync('mboost.me.key'),
    cert: fs.readFileSync('mboost.me.crt')
};

let server = null

try {
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
catch (exception) {
    console.error(`There was an error attempting to create the HTTPS server: ${exception}`);
    console.log('You should probably report this error to MainDab staff. Press any key to exit...');
    process.stdin.setRawMode(true);
    process.stdin.resume();
    process.stdin.on('data', process.exit.bind(process, 0));
    return;
}

try {
    server.listen(443, '127.0.0.1', () => {
        console.log('MainDab WeAreDevs API Bypass\nHTTPS server running on https://127.0.0.1:443 and listening for requests from WRD');
        console.log(`Press Ctrl+C to stop the server and exit (though you probably don't want to do this)`);
    });
} 
catch (exception) {
    console.error(`There was an error attempting to listen on port 443: ${exception}`);
    console.log('You should probably report this error to MainDab staff. Press any key to exit...');
    process.stdin.setRawMode(true);
    process.stdin.resume();
    process.stdin.on('data', process.exit.bind(process, 0));
    return;
}

process.on('uncaughtException', function (exception) {
    console.error(`An uncaught exception occurred: ${exception}`);
    console.log('Press any key to exit...');
    process.stdin.setRawMode(true);
    process.stdin.resume();
    process.stdin.on('data', process.exit.bind(process, 0));
});