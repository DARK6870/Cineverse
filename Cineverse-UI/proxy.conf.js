const PROXY_CONFIG = {
  "/api": {
    "target": "http://localhost:7404/api/graphql",
    "secure": false,
    "changeOrigin": true,
    "logLevel": "debug"
  }
};

module.exports = PROXY_CONFIG;
