var fileProvider = require('enb/techs/file-provider'),
    enbBemTechs = require('enb-bem-techs'),
    bemhtml = require('enb-bemxjst/techs/bemhtml');

module.exports = function(config) {

    config.nodes('bundles/*', function(nodeConfig) {

        nodeConfig.addTechs([
            [fileProvider, { target: '?.bemdecl.js' }],
            [enbBemTechs.levels, { levels: ['blocks'] }],
            [enbBemTechs.files],
            [enbBemTechs.deps],
            [bemhtml]
        ]);

        nodeConfig.addTargets(['?.bemhtml.js']);
    });
};