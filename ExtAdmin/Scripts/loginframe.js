Ext.onReady(function () {
    Ext.create('Ext.form.Panel', {
        title: '登录到 ExtAdmin',
        bodyPadding: 5,
        width: 500,
        autoHeight: true,
        // The form will submit an AJAX request to this URL when submitted
        url: '/handler/login.ashx',
        method: 'login',
        // style: { margin: '50 auto' },

        // Fields will be arranged vertically, stretched to full width
        layout: 'anchor',
        defaults: {
            anchor: '100%'
        },

        // The fields
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
            formBind: true, //only enabled once the form is valid
            disabled: true,
            handler: function () {
                var form = this.up('form').getForm();
                if (form.isValid()) {
                    form.submit({
                        success: function (form, action) {
                            Ext.Msg.alert('登录', action.result.msg);
                            document.cookie = "ConnectionString="+action.result.constr+"";
                            document.location = "/default.aspx";
                            //console.log(action);
                        },
                        failure: function (form, action) {
                            Ext.Msg.alert('登录', action.result.msg);
                        }
                    });
                }
            }
        }],
        renderTo: 'LoginFrame'
    });
});