function editLabelSave(textbox,causesValidation,validationGroup,imageUrlDuringCallBack,textDuringCallBack,enabledDuringCallBack,preCallBackFunction,postCallBackFunction,callBackCancelledFunction) {
	var preProcessOut = new Anthem_PreProcessCallBackOut();
	var preProcessResult = Anthem_PreProcessCallBack(textbox,null,null,causesValidation,validationGroup,imageUrlDuringCallBack,textDuringCallBack,enabledDuringCallBack,preCallBackFunction,callBackCancelledFunction,preProcessOut);
    if (preProcessResult) {
        Anthem_InvokeControlMethod(
            textbox.id,
            'SaveLabel',
            [textbox.value,true],           
            function(result) {
                Anthem_PostProcessCallBack(result,textbox,null,null,
                    function (result) {},
                    null,imageUrlDuringCallBack,textDuringCallBack,postCallBackFunction,preProcessOut);
            }, 
            null
        );
    }
}
function editLabelCheckKey(e) {
    return (e.keyCode!=13);
}
