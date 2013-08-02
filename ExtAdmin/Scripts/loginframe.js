Ext.onReady(function () {
    Ext.direct.Manager.addProvider(Ext.app.REMOTING_API);
    Ext.create('Ext.form.Panel', {
        title: '登录到 ExtAdmin',
        bodyPadding: 5,
        width: 500,
        autoHeight: true,
        layout: 'anchor',
        defaults: {
            anchor: '100%'
        },
        defaultType: 'textfield',
        items: [{
            fieldLabel: '服务器',
            name: 'server',
            allowBlank: false
        }, {
            fieldLabel: '用户名',
            name: 'username',
            allowBlank: false
        }, {
            fieldLabel: '密  码',
            name: 'password',
            inputType: 'password',
            allowBlank: true
        }],
        buttons: [{
            text: '重填',
            handler: function () {
                this.up('form').getForm().reset();
            }
        }, {
            text: '登录',
            formBind: true,
            disabled: true,
            handler: function () {
                var form = this.up('form').getForm();
                if (form.isValid()) {
                    PageInterface.login(form.getValues(), function (result, b, c) {
                        if (result.success) {
                            //Ext.Msg.alert('登录', result.msg);
                            Ext.util.Cookies.set('ConnectionString', result.constr);
//                            document.cookie = "ConnectionString=" + result.constr + "";
                            document.location = "/default.aspx";
                        } else {
                            Ext.Msg.alert('登录', result.msg);
                        }
                    });
                }
            }
        }],
        renderTo: 'LoginFrame'
    });

    //    Ext.create('Ext.form.Panel', {
    //        title: '登录到 ExtAdmin',
    //        bodyPadding: 5,
    //        width: 500,
    //        autoHeight: true,
    //        url: '/handler/login.ashx',
    //        method: 'login',

    //        layout: 'anchor',
    //        defaults: {
    //            anchor: '100%'
    //        },

    //        defaultType: 'textfield',
    //        items: [{
    //            fieldLabel: '服务器',
    //            name: 'server',
    //            allowBlank: false
    //        }, {
    //            fieldLabel: '用户名',
    //            name: 'username',
    //            allowBlank: false
    //        }, {
    //            fieldLabel: '密  码',
    //            name: 'password',
    //            inputType: 'password',
    //            allowBlank: true
    //        }],
    //        buttons: [{
    //            text: '重填',
    //            handler: function () {
    //                this.up('form').getForm().reset();
    //            }
    //        }, {
    //            text: '登录',
    //            formBind: true, 
    //            disabled: true,
    //            handler: function () {
    //                var form = this.up('form').getForm();
    //                if (form.isValid()) {
    //                    form.submit({
    //                        success: function (form, action) {
    //                            Ext.Msg.alert('登录', action.result.msg);
    //                            document.cookie = "ConnectionString="+action.result.constr+"";
    //                            document.location = "/default.aspx";
    //                        },
    //                        failure: function (form, action) {
    //                            Ext.Msg.alert('登录', action.result.msg);
    //                        }
    //                    });
    //                }
    //            }
    //        }],
    //        renderTo: 'LoginFrame'
    //    });
});