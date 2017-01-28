var bemhtml = require('./bundles/index/index.bemhtml');

module.exports = function (callback, json) {
    try {
        var obj = JSON.parse(json);
        callback(null, bemhtml.BEMHTML.apply(obj));
    } catch (err) {
        callback(err);
    }
};