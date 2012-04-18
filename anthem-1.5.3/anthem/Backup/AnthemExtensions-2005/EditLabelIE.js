function editLabelEdit(label,causesValidation,validationGroup,imageUrlDuringCallBack,textDuringCallBack,enabledDuringCallBack,preCallBackFunction,postCallBackFunction,callBackCancelledFunction) {
	var preProcessOut = new Anthem_PreProcessCallBackOut();
	var preProcessResult = Anthem_PreProcessCallBack(label,null,null,causesValidation,validationGroup,imageUrlDuringCallBack,textDuringCallBack,enabledDuringCallBack,preCallBackFunction,callBackCancelledFunction,preProcessOut);
    if (preProcessResult) {
        Anthem_InvokeControlMethod(
            label.id,
            'EditIELabel',
            [],           
            function(result) {
                Anthem_PostProcessCallBack(result,label,null,null,null,null,imageUrlDuringCallBack,textDuringCallBack,postCallBackFunction,preProcessOut);
            }, 
            null
        );
    }
}
function editLabelSave(label,causesValidation,validationGroup,imageUrlDuringCallBack,textDuringCallBack,enabledDuringCallBack,preCallBackFunction,postCallBackFunction,callBackCancelledFunction) {
	var preProcessOut = new Anthem_PreProcessCallBackOut();
	var preProcessResult = Anthem_PreProcessCallBack(label,null,null,causesValidation,validationGroup,imageUrlDuringCallBack,textDuringCallBack,enabledDuringCallBack,preCallBackFunction,callBackCancelledFunction,preProcessOut);
    if (preProcessResult) {
        Anthem_InvokeControlMethod(
            label.id,
            'SaveLabel',
            [label.innerHTML,false],           
            function(result) {
                Anthem_PostProcessCallBack(result,label,null,null, 
                    function (result) {
                        label.innerHTML = result.value.Text;
                    },
                    null,imageUrlDuringCallBack,textDuringCallBack,postCallBackFunction,preProcessOut);
            }, 
            null
        );
    }
}

