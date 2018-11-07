var system = ['report/'],
    interUrl = {
        mockList: "",
        basic: "/",
        common: {
            login: "login",
            getProvince: "area/getProvince",
            getCity: "area/getCityByProvince",
            getArea: "area/getCountyByCity",
            getCompany: "carDealer/branchComp/list",
            getGroup: "group/list",
            getGroupsByCompanyIds: "group/getGroupsByCompanyIds",
            branchAndGroupComo: "carDealer/branchAndGroupComp/list",
            headAndBranchComp: "carDealer/headAndBranchComp/list",
            orgInfo: "org/info",
            orgList: "org/list",
            getDepart: "carDealer/department/list", //根据公司获取部门
            getBizDepart: "bizDepart/user/list", //根据部门获取人员
            getUserById:"uid/user/list",//根据id查询用户
            ruleList: "role/list",
            brandList: "brandinfo/getbrandinfobychar",
            carList: "brandinfo/getbrandinfobycode",
            carModels: "brandinfo/getcarname",
            opinion: "opinion/save",
            opinionOnly: "opinion/only",
            getOpinion: "opinion/get",
            approveOpinion: "opinion/approveOpinion",
            loanCarList: "cooperation/carDealer/list",
            insuranceList: "cooperation/insurance/list",
            bankInfo: "cooperation/bank/info",
            flowGet: "flow/get",
            getLoanFlowStatusList: "infoQuery/getLoanFlowStatusList",
            getAreaFullNameByAreaName: "loanCarPlateArea/getAreaFullNameByAreaName",
            cooperation: "cooperation/thirdCooperation/list",
            //获取图片信息
            getImgInfor: "user/register",
            report: "vistis/report",
            secondCarAssessment: "estimate/getByApplyId",
            secondCarVin:"estimate/secondCarVin",
            secondCarById:"estimate/secondCarById",
            loanApplyBankList:"loanApply/bank/list",
            getRiskRule: "loanRiskRules/getRiskRule",
            getRiskRuleDetails: "loanRiskRules/getRiskRuleDetails",
            faceList: "loanRiskRules/faceList",
            faceComparision: "loanRiskRules/faceComparision"
        },
        user: {
            login: "login",
            getUser: "user/session/get",
            menu: "za/menu/list",
            logOut: "logout"
        },
        purchase: {
            purchaselist: "purchase/apply/list",
            purchaseadd: "purchase/apply/add",
            purchasedelete: "purchase/apply/delete",
            purchaseupdate: "purchase/apply/update",
            purchaseget: "purchase/apply/get",
            purchaseaddGet: "purchase/apply/addGet",
            contractlist: "purchase/contract/list",
            contractadd: "purchase/contract/add",
            contractdelete: "purchase/contract/delete",
            contractget: "purchase/contract/get",
            contractupdate: "purchase/contract/update",
            contractaddGet: "purchase/contract/addGet",
            contractgetDetail: "purchase/contract/getDetail",
            notStocklist: "purchase/stock/purchaseStockList",
            stockList: "purchase/stock/stockList",
            stockaddGet: "purchase/stock/addGet",
            useNotList: "purchase/stock/useNotList",
            useList: "purchase/stock/useList",
            userStockList: "purchase/stock/userStockList",
            stockadd: "purchase/stock/addStock",
            getDetail: "purchase/stock/getDetail",
            saveStock: "purchase/stock/saveStock",
            stockDel: "purchase/stock/delStock",
            updatecontractAmt :"purchase/contract/updatecontractAmt"
        },
        customer: {
        	switchCreateCreditInfo: "customer/import/switchCreateCreditInfo",
            list: "customer/import/list",
            get: "customer/import/get",
            close: "customer/import/close",
            reject: "customer/import/reject",
            branchReject: "customer/import/branchReject",
            groupList: "customer/import/list1",
            branchList: "customer/import/list",
            allot: "customer/import/allot",
            groupBranch: "customer/import/setBranch",
            getVisitAddressGPSInfo: "infoQuery/getVisitAddressGPSInfo",
            query:"customer/query",
            queryGpsInstall: "gps/queryGpsInstall",
            queryCheckGpsInstall:"gps/queryCheckGpsInstall",
            gpsInstallExport: "gps/queryGpsInstallExport",
            queryGpsCheckExport: "gps/queryGpsCheckExport",
            myGpsRead: "gps/myGpsRead",
            orderPush: "gps/orderPush",
            updateOrderState: "gps/updateOrderState",
            updateReviewState: "gps/updateReviewState",
            orderFixTerminal: "gps/orderFixTerminal",
            getGpsInfo: "gps/getGpsInfo",
            currentNodeNameQuery: "gps/currentNodeNameQuery"
        },
        gr: {
            getPhoto: "photoPreview/getAllDocInfo",
            list: "customer/list",
            add: "customer/add",
            update: "customer/update",
            get: "customer/get",
            delete: "customer/del",
            history: "customer/history",
            teamList: "customer/manager/list",
            customerAssetList: "customer/asset/list",
            relationShipList: "customer/relationship/list",
            customerManagerDel: "customer/manager/del",
            customerStatus: "customer/manager/setStatus",
            customerSetAut: "customer/manager/setAuth",
            customerRelationDel: "customer/relationship/del",
            customerRelationAdd: "customer/relationship/add",
            customerRelationEdit: "customer/relationship/edit",
            documentDir: "loanApprovalInfo/getApprovalDocumentDir",
			documentUpdateResult: "loanDocumentCheck/updateResult",
			documentExport: "loanDocumentCheck/export",
			documentGetCheckStatus: "loanDocumentCheck/getCheckStatus",
            //documentAllDir: "loanApprovalInfo/getAllApprovalDocumentDir",
            recordDocQueryHistory: "photoPreview/recordDocQueryHistory",
            readPhoto:"photoPreview/readPhoto",
            documentAllDir: "loanApprovalInfo/getApprovalDocumentDir",
            documentList: "loanApprovalInfo/getApprovalDocument",
            documentCopy: "loanDocument/copyFile",
            assetDistribution: "loanDocument/assetDistribution",
            recoveryFile: "loanDocument/recoveryFile",
            moveDocument: "loanDocument/moveFile",
            delDocument: "loanDocument/deleteFile",
            loanList: "customer/loan/list",
            loanInfo: "loanApprovalInfo/getApprovalBudgetInfo",
            userList: "user/list",
            managerAdd: "customer/manager/add",
            loanApprovalInfo: "loanApprovalInfo/getApprovalAsserts",
            getApprovalList: "loanApprovalInfo/getApprovalGuarantor",
            getLoanGuarantorInfo: "loanInside/getLoanGuarantorInfo",
            dGetLoanGuarantorInfo: "deliver/getLoanGuarantorInfo",
            lauchLoanGuarantorInfo: "loanInside/lauchLoanGuarantorInfo",
            uploadImage: "loanDocument/uploadFileString",
            upFile: "loanDocument/uploadFileByStream",
            downLoad: "loanDocument/downloadAllFile",
            flow: "flow",
            record: "customer/history/detail",
            loanQuery: "infoQuery/loanInfo",
            getLoanModifyHistory: "infoQuery/getLoanModifyHistory",
            creditList: "loanApply/creditList",
            customerCreditPassList:"loanApply/creditPassCustomerList",
            vilidateCreditPass:"loanApply/vilidateCreditPass",
            changeSpouse:"loanApply/changeSpouse", //更新客户与配偶的关系
            IsNewAddSpouse:"loanApply/IsNewAddSpouse", //增加配偶信息判断
            addSpouse:"loanApply/addSpouse",//新增配偶
            carDealerList: "cooperation/carDealer/list",
            noDeletedCarDerlerList:"cooperation/carDealer/noDeletedCarDerlerList",
            bankList: "cooperation/bank/list",
            openingBanks:"chengyirong/getopeningBanks",
            getOpeningBank: "cooperation/openingBank/all",
            flowUser: "flowUser",
            loanInfoList: "infoQuery/loanInfo",
            getLoanModifyList: "loanModify/getLoanModifyList/modify",
            getLoanCancelList: "loanModify/getLoanModifyList/cancel",
            launchLoanModifyApply: "loanModify/launchLoanModifyApply",
            checkLaunchCancel: "loanModify/checkLaunchCancel",
            expressCompanyCode: "cooperation/codeLibrary/list",
            reditEdit: "customer/credit/editRelavants",
            back2pre: "credit/back2pre",
            getLoanInfoOverview: "infoQuery/getLoanInfoOverview",
            getFlowPath: "infoQuery/getFlowPath",
            patchSearch: "patch/search",
            getBankAll: "cooperation/bank/all",
            getTemplateList: "loanOverdueLoadHis/getTemplateList",
            loanOverdueLoadHisList: "loanOverdueLoadHis/list",
            uploadExcel: "loanOverdueLoadHis/uploadExcel",
            loanOverdueLoadHisFinish: "loanOverdueLoadHis/finish",
            loanOverdueLoadHisRematch: "loanOverdueLoadHis/rematch",
            loanOverdueLoadHisDelete: "loanOverdueLoadHis/delete",
            getMatchFieldList: "loanOverdueLoadHis/getMatchFieldList",
            judgePictureNumber: "loanDocument/judgePictureNumber",
            financialPlanList:"addFinancialPlan/addFinancialPlanList",
            getaddFinancialPlan:"addFinancialPlan/getaddFinancialPlan",
            allAddFinancialPlanList:"addFinancialPlan/allAddFinancialPlanList",
            stopOrderReceiveList:"orderReceive/stopOrderReceiveList",
            stopOrderReceiveApply:"orderReceive/stopOrderReceiveApply",
            stopOrderReceiveApprovers:"orderReceive/stopOrderReceiveApprovers",
            stopOrderReceiveApprove:"orderReceive/stopOrderReceiveApprove",
            stopOrderReceiveApproveList:"orderReceive/stopOrderReceiveApproveList",
            recoverOrderReceiveApply:"orderReceive/recoverOrderReceiveApply",
            getUserOrderRecieveStatus:"orderReceive/getUserOrderRecieveStatus",
            saveCheckResult:"upLoanAmountCheck/saveCheckResult",
            updateCheckResult:"upLoanAmountCheck/updateCheckResult",
            getCheckResultByApplyId:"upLoanAmountCheck/getCheckResultByApplyId"
        },
        credit: {
            creditList: "customer/credit/list",
            creditInfo: "customer/credit/get",
			loanDocumentCheckList: "loanDocumentCheck/list",
            loanCreditInfo: "loanApprovalInfo/getCustomerCreditInfo",
            customerGet: "customer/get",
            creditAdd: "customer/credit/add",
            creditPreAdd: "customer/credit/preAdd",
            creditUser: "loanApply/list",
            creditRisk: "customer/credit/risk/list",
            creditSubmit: "credit/preSubmit",
            creditImport: "customer/import/get",
            creditSubmit2: "credit/submit2next",
            creditEdit: "customer/credit/editRelavants",
            addRelavant: "customer/credit/addRelavant",
            addCreditFile: "customer/credit/addCreditFile",
            delCreditFile: "customer/credit/delCreditFile",
            delRelavant: "customer/credit/delRelavant",
            back2pre: "credit/back2pre",
            cancel: "credit/cancel",
            CustomerCreditList: "infoQuery/getCustomerCreditList",
            getCustomerCreditListByProjectId: "loanApprovalInfo/getCustomerCreditList",//参数都是loanApplyId,creditId,projectId(自动转换)
            //getCustomerCreditListByProjectId: "infoQuery/getCustomerCreditListByProjectId",//参数都是loanApplyId,creditId,projectId(自动转换)
            determined: "customer/credit/determined",
            download: "customer/credit/file/download",
            addCreditReport:"customer/credit/addCreditReport"
        },
        myTask: {
            TaskList: "mytasks",
            searchTaskList: "mytasks/search",
            getAssign: "loanAssign/getAssign",
            editAssign: "loanAssign/saveAssign",
            listsAssignMain: "loanAssign/listsAssign/main",
            listsAssignVice: "loanAssign/listsAssign/vice",
            getSurvery: "loanAssign/getSurvery",
            editLoanerInfo: "loanInside/editLoanerInfo",
            saveBudgetInfo: "loanInside/saveBudgetInfo",
            saveLoanBaseInfo: "loanInside/saveLoanBaseInfo",
            getLoanBaseInfo: "loanInside/getLoanBaseInfo",
            getLoanCarPrice: "loanInside/getLoanCarPrice",
            roleLists: "role/list",
            approvalInfo: "loanApprovalInfo/getApprovalInfo",
            chengyirongApprovalInfo:"loanApprovalInfo/getChengyirongApprovalInfo",
            approvalBaseInfo: "loanApprovalInfo/getApprovalBaseInfo",
            chengyirongApprovalBaseInfo: "loanApprovalInfo/getChengyirongApprovalBaseInfo",
            occupationList: "cooperation/codeLibrary/list", //职业
            unitList: "cooperation/codeLibrary/list", //单位经济性质
            jobList: "cooperation/codeLibrary/list", //职务
            customerContacter: "loanApprovalInfo/getLoanCustomerContacter",
            approvalBudgetInfo: "loanApprovalInfo/getApprovalBudgetInfo",
            chengyirongApprovalBudgetInfo: "chengyirongLoanApply/getApprovalBudgetInfo",
            approvalAsserts: "loanApprovalInfo/getApprovalAsserts",
            approvalGuarantor: "loanApprovalInfo/getApprovalGuarantor",
            paymentGet: "loan/payment/get",
            guaranteeList: "cooperation/guarantee/list",
            back2pre: "loanReview/back2pre",
            chengyirongBack2pre: "chengyirongLoanReview/back2pre",
            preSubmit: "loanReview/preSubmit",
            submit2next: "loanReview/submit2next",
            back2BudgetOfficeStaff: "loanReview/back2BudgetOfficeStaff",
            closeLoanApply: "loanReview/closeLoanApply",
            preLongTop: "loanReview/preLongTop",
            submit2LongTop: "loanReview/submit2LongTop",
            deleteLoanCustomerContacter: "loanInside/deleteLoanCustomerContacter",
            modifyLoanCustomerContacter: "loanInside/modifyLoanCustomerContacter",
            saveLoanCustomerContacter: "loanInside/saveLoanCustomerContacter",
            paymentSave: "loan/payment/saveToCarDealer",
            relateLoanGuarantor: "loanInside/relateLoanGuarantor",
            saveLoanGuarantorInfo: "loanInside/saveLoanGuarantorInfo",
            dSaveLoanGuarantorInfo: "deliver/saveLoanGuarantorInfo",
            deleteLoanGuarantorInfo: "loanInside/deleteLoanGuarantorInfo",
            addLoanGuarantorInfo: "loanInside/addLoanGuarantorInfo",
            savePaymentRequest:"loan/payment/savePaymentRequest",
            saveToGuarantee: "loan/payment/saveToGuarantee",
            paymentGetGuarantee: "loan/payment/getGuaranteePayment",
            getCarDealerPayment: "loan/payment/getCarDealerPayment",
            getAccountList: "cooperation/carDealer/account/list",
            getCapatilPoolAccountList: "cooperation/getCapatilPoolAccountList",
            printBudgetInfo: "loanInside/printBudgetInfo",
            getApprovalOtherInfo: "loanApprovalInfo/getApprovalOtherInfo",
            getLoanCollection: "loan/payment/getLoanCollection",
            saveLoanCollection: "loan/payment/saveLoanCollection",
            myTasksRead: "mytasks/read",
            reverseSpouseInfo: "loanInside/reverseSpouseInfo",
            updateCustomerCredit: "loanApply/updateCustomerCredit",
            isReg: "loanApprovalInfo/getContractsInfo",
            back2LoanLaunch: "loanReview/back2LoanLaunch",
            back2ChengyirongLoanLaunch: "chengyirongLoanReview/back2LoanLaunch",
            need2Door: "loanApply/need2Door",
            getDocBudgetInfo: "infoQuery/getLoanFeeInfo",
            deGetApprovalFeeInfo: "deliver/getApprovalFeeInfo",
            deGetSaveFeeInfo: "deliver/saveFeeInfo",
            emergencyContactShow: "loanApprovalInfo/emergencyContactShow",
            validatePayInfo: "loan/payment/validatePayInfo",
            validateEditPassWordAuth : "loan/payment/validateEditPassWordAuth",
            chengyirongPreSubmit: "chengyirongLoanReview/preSubmit",
            chengyirongSubmit2next: "chengyirongLoanReview/submit2next"
        },
        loanDetail: { 
            launch: "loanApply/launch",
            loanGet: "loanApply/get",
            getMinBillingPrice: "loanApply/getMinBillingPrice",
            loanUpdate: "loanApply/update",
			verify: "loanReview/verify",
            loansave: "loanApply/save",
            loansubmit: "loanReview/preSubmit",
            loanCarList: "cooperation/carDealer/list",
            loanList: "loanApply/creditList",
            loanInfo: "loanInfo/getLoanCarInfoAndLicensePlateInfo",
            getLoanContractInfo: "loanInfo/getLoanContractInfo",
            loanReview: "loanReview/cancel",
            getLoanProjectNo: "loanApply/getLoanProjectNo",
            getLoanFeeInfo: "loanApply/getLoanFeeInfo",
            getFinancialProduct: "loanApply/getFinancialProduct",
            getSecondHandCarList: "loanApply/getSecondHandCarList",
            getLoanCarInfoAndLicensePlateInfo: "infoQuery/getLoanCarInfoAndLicensePlateInfo",
            getPledgeInfoInfo: "infoQuery/getPledgeInfoInfo",
            getBankRemittanceInfo: "infoQuery/getBankRemittanceInfo",
            getRepaymentCardInfo: "infoQuery/getRepaymentCardInfo",
            getLoanDocumentTransmitList: "infoQuery/getLoanDocumentTransmitList",
            infoQueryGetLoanContractInfo: "infoQuery/getLoanContractInfo",
            getLoanContractRepayPlanList: "infoQuery/getLoanContractRepayPlanList",
            getLoanInsuranceInfoList: "infoQuery/getLoanInsuranceInfoList",
            getLoanCustomerInfo: "infoQuery/getLoanCustomerInfo",
            getLoanFeeInfoInfoQuery: "infoQuery/getLoanFeeInfo",
            getLoanAssetsInfo: "infoQuery/getLoanAssetsInfo",
            getLoanGuarantorInfo: "infoQuery/getLoanGuarantorInfo",
            getLoanApplyFlowList: "infoQuery/getLoanApplyFlowList",
            getGpsTrajectoryParam: "infoQuery/getGpsTrajectoryParam",
            getLoanCustomerContacter: "infoQuery/getLoanCustomerContacter",
            flowLoan: "flow/loan",
            chengyirongLaunch:"chengyirongLoanApply/launch",
            chengyirongLoansave:"chengyirongLoanApply/save",
            chengyirongLoanGet: "chengyirongLoanApply/get",
            chengyirongLoanUpdate:"chengyirongLoanApply/update",
            chengyirongVerify: "chengyirongLoanReview/verify",
            chengyirongLoansubmit: "chengyirongLoanReview/preSubmit",
            taskLicensePlateInfoUp: "licensePlateInfoVo/taskLicensePlateInfoUp",
            licensePreSubmit: "licensePlateInfo/process/preSubmit",
            licenseSubmit2next: "licensePlateInfo/process/submit2next",
            licenseBack2pre: "licensePlateInfo/process/back2pre",
            licenseFinish: "licensePlateInfo/process/finish",
            pledgePreSubmit: "pledgeInfo/process/preSubmit",
            pledgeSubmit2next: "pledgeInfo/process/submit2next",
            pledgeBack2pre: "pledgeInfo/process/back2pre",
            pledgeFinish: "pledgeInfo/process/finish"
            
        },
        carDealer: {
        	jiarongselect:"carDealer/addFinancialPlan/select",
            get: "carDealer/get",
            add: "carDealer/add",
            update: "carDealer/update",
            setStatus: "carDealer/setStatus",
            list: "carDealer/list",
            manager: "carDealer/manager/list",
            userList: "carDealer/manager/user/list",
            managerAdd: "carDealer/manager/add",
            managerSet: "carDealer/manager/setManager",
            accountList: "carDealer/account/list",
            accountAdd: "carDealer/account/add",
            accountDel: "carDealer/account/del",
            accountStop: "carDealer/account/setStatus",
            verifyAccountCanOper: "carDealer/verifyAccountCanOper",
            delete: "carDealer/del",
            fee: "carDealer/fee/list",
            feeGet: "carDealer/fee/get",
            feeAdd: "carDealer/fee/add",
            feeUpdate: "carDealer/fee/update",
            feeStop: "carDealer/fee/stop",
            feePromptMsg:"carDealer/fee/promptMsg",
            isManager: "carDealer/isManager",
            managerDel: "carDealer/manager/del",
            managerSetStatus: "carDealer/manager/setStatus",
            preSubmit: "carDealer/process/preSubmit",
            submit2next: "carDealer/process/submit2next",
            back2pre: "carDealer/process/back2pre",
            cancel: "carDealer/process/cancel",
            carDealerInit: "carDealer/init",
            carDealerPyGet: "carDealer/py/get",
            carOrderList: "car/order/list",
            carOrderGrabOrder: "car/order/grabOrder",
            carOrderBackOrder: "car/order/backOrder",
            carDealerFeeCopy: "carDealer/fee/copy",
            carDealerFeeDelete: "carDealer/fee/delete",
            queryReportByReportId:"carDealer/queryReportByReportId",
            //信息更正
            carDealerInfoCorrect: "carDealer/manager/syncGroup",
            carDealerBack2launch: "carDealer/process/back2launch",
            setDefault: "carDealer/account/setDefault",
            selectList: "carDealer/selectList",
            carDealerScore: "cardealer/grade/detail",
            carDealerScoreLatestSixMonth: "cardealer/grade/latestSixMonth",
            carDealerBusinessQuery:"carDealer/fee/carDealerBusinessQuery",
            carDealerBusinessCountQuery:"carDealer/fee/carDealerBusinessCountQuery",
            carDealerBusinessQueryExport : "carDealer/fee/carDealerBusinessQueryExport"
            //===============展业区域接口地址  2016-11-07==================
            /*zhanyeList: "carDealer/zhanye/list",
            zhanyeAdd: "carDealer/zhanye/add",
            zhanyeDel: "carDealer/zhanye/del"*/
        },
        insurance: {
            loanInsuranceList: "loanInsuranceInfo/loanInsuranceList",
            loanInsuranceInfoAdd: "loanInsuranceInfo/add",
            loanInsuranceTypeList: "loanInsuranceInfo/typeList",
            loanInsuranceInfoList: "loanInsuranceInfo/loanInsuranceInfoList",
            loanInsuranceInfoDel: "loanInsuranceInfo/delete",
            loanInsuranceInfoToUpdate: "loanInsuranceInfo/toUpdate",
            loanInsuranceInfoUpdate: "loanInsuranceInfo/update",
            getLoanInsuranceInfo: "infoQuery/getLoanInsuranceInfo",
            getLoanInsuranceInfoTypeList: "infoQuery/getLoanInsuranceInfoTypeList",
            //续保列表接口
            getRenewInsuranceList: "loanInsuranceRenewal/renewalProjectList",
            getRenewInsuranceListInfo: "loanInsuranceRenewal/list",
            getRenewInsuranceListPhone: "loanInsuranceRenewal/listPhone",
            //删除记录
            delInsuranceRenew: "loanInsuranceRenewal/delete",
            delInsuranceRenewPhone: "loanInsuranceRenewal/deletePhone",
            //修改记录
            modifyInsuranceRenew: "loanInsuranceRenewal/update",
            modifyInsuranceRenewPhone: "loanInsuranceRenewal/updatePhone",
            //添加记录
            addInsuranceRenew: "loanInsuranceRenewal/add",
            addInsuranceRenewPhone: "loanInsuranceRenewal/addPhone",
            //获取记录
            getInsuranceRenew: "loanInsuranceRenewal/toUpdate",
            getInsuranceRenewPhone: "loanInsuranceRenewal/toUpdatePhone",
            //获取图片列表
            getInsuranceRenewImags: "loanApprovalInfo/getApprovalDocument",
            //获取联系信息
            getCOntact: "loanInsuranceRenewal/getSpouseInfo",
            //保险核销
            loanInsuranceRenewalInfoCavList: "loanInsuranceRenewal/loanInsuranceRenewalInfoCavList",
            //核销导出
            loanInsuranceRenewalInfoCavListExport: "loanInsuranceRenewal/loanInsuranceRenewalInfoCavListExport",
            updateCavStatus: "loanInsuranceRenewal/updateCavStatus"
        },
        asset: {
            //loanAssetPackageList: "loanAssetPackage/list",
            loanAssetPackageSave: "loanAssetPackage/save",
            loanAssetPackageDel: "loanAssetPackage/del",
            loanAssetPackageGet: "loanAssetPackage/get",
            loanAssetList: "loanAssetPackageManage/addAssets/list",
            loanAssetAdd: "loanAssetPackageManage/addAssets/add",
            loanAssetDel: "loanAssetPackageManage/delAssets",
            lockAssetsPackage: "loanAssetPackageManage/lockAssetsPackage",
            getInAssetsPackage: "loanAssetPackageManage/getListInAssetsPackage",
            getLoanApproveProject: "loanAssetPackageManage/getLoanApproveProject",
            getLoanApproveCustomer: "loanAssetPackageManage/getLoanApproveCustomer",
            getAssetPackageNo: "loanAssetPackage/getAssetPackageNo",
            loanAssetPackageManage: "loanAssetPackage/manage/list",
            loanAssetPackage: "loanAssetPackage/search/list",
            export: "loanAssetPackageManage/export"
        },
        second: {
            estimateSearch: "estimate/search",
            serial: "estimate/serial",
            add: "estimate/add",
            getColor:"estimate/color",
            getBusinessTypes:"estimate/businessTypes",//业务品种
            preSubmit: "estimate/process/preSubmit",
            submit2next: "estimate/process/submit2next",
            cancel: "estimate/process/cancel",
            close: "estimate/process/close",
            get: "estimate/get",
            save: "opinion/save",
            save1th: "estimate/save1th",
            save2th: "estimate/save2th",
            estimate3th: "estimate/estimate3th",
            back2pre: "estimate/process/back2pre",
            transferBack2pre: "transfer/process/back2pre",
            not_yet: "transfer/search/not_yet",
            already: "transfer/search/already",
            saves: "transfer/save",
            transferPreSubmit: "transfer/process/preSubmit",
            transferSubmit2next: "transfer/process/submit2next",
            update: "estimate/update",
            opinionLast: "estimate/opinion/last",
            userList: "user/list",
            secondCarExport:"estimate/secondCarExport",
            cancelOrCloseCan:"estimate/cancelOrCloseCan",
            getReportAndEvaluateResult:"carDealer/getReportAndEvaluateResult"
        },
        messageBoardManage: {
            myQuestionList: "clsFeedbackManage/myQuestionList",
            clsFeedbackManageAdd: "clsFeedbackManage/add",
            feedbackDetail: "clsFeedbackManage/feedbackDetail",
            myFeedbackReply: "clsFeedbackManage/myFeedbackReply"
        },
        documentManagement: {
        	checkCancelProcess:"deliver/process/checkCancelProcess",
            deliverList: "deliver/list",
            sendCarList: "deliver/car/list",
            carList: "deliver/car/sendList",//待寄送
            sendCompanyList: "deliver/company/list",
            getByUidBank:"deliver/getconBankId",
            hasSendCompanyList: "deliver/company/sendList",
            sendBankList: "deliver/bank/list",
            hasSendBankList: "deliver/bank/sendList",
            expressList: "deliver/bill/detail",
            addExpress: "deliver/bill/add",
            saveExpress: "deliver/bill/save",
            approveExpress:"deliver/bill/approve",
            updateSendData:"deliver/updateBillById",
            delExpress: "deliver/bill/del",
            recipientBankList: "deliver/bank/recipientList",
            recipientCompanyList: "deliver/company/recipientList",
            getExpress: "deliver/bill/get",
            recipient: "deliver/recipient/do",
            recipientTemp: "deliver/recipient/temp",
            recipientCancel: "deliver/recipient/cancel",
            submit2next: "deliver/process/submit2next",
            deliverGet: "deliver/get",
            deliverStart: "deliver/start",
            deliverSave: "deliver/save",
            preSubmit: "deliver/process/preSubmit",
            cancel: "deliver/process/cancel",
            getApprovalProjectInfo: "infoQuery/getApprovalProjectInfo",
            deliverGetApprovalProjectInfo: "deliver/getApprovalProjectInfo",
            getLoanContractInfo: "infoQuery/getLoanContractInfo",
            back2pre: "deliver/process/back2pre",
            saveCar: "licensePlateInfoVo/saveCar",
            documentOpinionLast: "opinion/last",
            getDataAuditing: "deliver/getApprovalBaseInfo",
            saveCustomerInfo: "deliver/editLoanerInfo",
            cancelDeliver: "deliver/cancel",
            pigeonholeList: "deliver/pigeonholeList",
            pigeonhole: "deliver/pigeonhole", //确认归档
            prePigeonhole: "deliver/prePigeonhole"  //预归档
        },
        creditManagement: {
            licensePlateInfoVo: "licensePlateInfoVo/list",
            licenseAdd: "licensePlateInfoVo/save",
            licenseGet: "licensePlateInfoVo/get",
            getLicensePledgeModifyInfo: "licensePlateInfoVo/getLicensePledgeModifyInfo",
            mortageList: "pledgeInfo/list",
            mortageSave: "pledgeInfo/save",
            mortageGgt: "pledgeInfo/get",
            loanContractList: "loanContractInfo/list",
            loanContractSave: "loanContractInfo/save",
            loanContractGet: "loanContractInfo/get",
            loanContractEdit: "loanContractInfo/edit",
            loanContracPlanList: "loanContractInfo/plan/preList",
            bankList: "bankRemittanceInfo/list",
            bankInfoGet: "bankRemittanceInfo/get",
            bankInfoSave: "bankRemittanceInfo/save",
            bankCancel: "bankRemittanceInfo/cancel",
            repayCardList: "repaymentCardInfo/list",
            repayCardGet: "repaymentCardInfo/get",
            repayCardSave: "repaymentCardInfo/save",
            failRecordList: "licensePlateInfoVo/failRecordList",
            pledgeInfoFailRecordList: "pledgeInfo/failRecordList",
            isValid: "pageFieldValidConfig/isValid",
            tmpSave: "loanContractInfo/tmpSave"
        },
        loanCancel: {
            cancel: "loan/cancel/process/cancel",
            preSubmit: "loan/cancel/process/preSubmit",
            submit2next: "loan/cancel/process/submit2next",
            closeDocumentDeliver: "loan/cancel/process/closeDocumentDeliver",
            close: "loan/cancel/process/close",
            back2pre: "loan/cancel/process/back2pre"
        },
        loanModify: {
            cancel: "loan/modify/process/cancel",
            preSubmit: "loan/modify/process/preSubmit",
            submit2next: "loan/modify/process/submit2next",
            close: "loan/modify/process/close",
            back2pre: "loan/modify/process/back2pre",
            back2BudgetOfficeStaff: "loan/modify/process/back2BudgetOfficeStaff",
            getLoamModifyHistory: "loanModify/getLoamModifyHistory",
            getLoanRefundPayeeMethod: "loan/payment/getLoanRefundPayeeMethod",
            loanToBeInvalid: "loan/modify/process/toBeInvalid"
        },
        loanChange: {
            getLoanRefundFull: "loan/payment/getLoanRefundFull",
            getLoanRefundBalance: "loan/payment/getLoanRefundBalance",
            saveLoanRefundFull: "loan/payment/saveLoanRefundFull", //全额保存
            saveLoanRefundBalance: "loan/payment/saveLoanRefundBalance" //差额保存
        },
        report: {
        	customersReturnedExport: system[0] + "query/customersReturnedExport",
        	customersReturnedStatisQuery: system[0] + "query/customersReturnedStatis",
        	customersReturnedQuery: system[0] + "query/customersReturned",
        	marketingRebateExport: system[0] + "query/marketingRebateExport",
        	marketingRebateStatisQuery: system[0] + "query/marketingRebateStatis",
        	marketingRebateQuery: system[0] + "query/marketingRebate",
            businessQuery: system[0] + "query/business",
            queryBusinessStatis:system[0]+"query/businessStatis",
            feeQuery: system[0] + "query/fee",
            statisticByMonth: system[0] + "query/statisticByMonth",
            unPledgeStatistic: system[0] + "query/unPledgeStatistic",
            flowStatusStatistic: system[0] + "query/flowStatusStatistic",
            dealerCompanystatistic: system[0] + "query/dealerCompanystatistic",
            loanSignStatistic: system[0] + "query/loanSignStatistic",
            bankPayStatistic: system[0] + "query/bankPayStatistic",
            loanAmmountRotaryStatistic: system[0] + "query/loanAmmountRotaryStatistic",
            statisticRankWithSign: system[0] + "query/statisticRankWithSign",
            unPledgeRank: system[0] + "query/unPledgeRank",
            loanSignRank: system[0] + "query/loanSignRank",
            bankPayRank: system[0] + "query/bankPayRank",
            loanAmmountRotaryRank: system[0] + "query/loanAmmountRotaryRank",
            loanSignStatisticByArea: system[0] + "query/loanSignStatisticByArea",
            loanSignStatisticWithRecentPeriodByArea: system[0] + "query/loanSignStatisticWithRecentPeriodByArea",
            //报表接口
            carDealerInfoReportExport: system[0] + "carDealerInfoReport/export",
            carDealerInfoReportList: system[0] + "carDealerInfoReport/list",
            customerNumtatistic: system[0] + "query/customerNumtatistic",
            businessObjectProcessInfoReportList: system[0] + "businessObjectProcessInfoReport/list",
            businessExport: system[0] + "query/businessExport",
            businessObjectProcessInfoReportListExport: system[0] + "businessObjectProcessInfoReport/listExport",
            businessObjectProcessInfoReportLlistDetailExport:system[0]+"businessObjectProcessInfoReport/listDetailExport",
            queryStatisCustomerNum:system[0]+"query/statisCustomerNum",
            businessObjectProcessInfoReportListStatis:system[0]+"businessObjectProcessInfoReport/listStatis",
            carDealerInfoReportListStatis:system[0]+"carDealerInfoReport/listStatis",
            creditQuery:system[0] +"query/selectCredit",
            exportCredit:system[0] +"query/exportCredit",
            totalCredit:system[0] +"query/totalCredit",
            countCompanyPayment:system[0] +"query/countCompanyPayment",
            countBankPayment:system[0] +"query/countBankPayment"
        },
        export: {
            getOrg: "org/list",
            getBank: "cooperation/bank/all",
            getAllWithoutStatus: "cooperation/bank/getAllWithoutStatus",
            searchList: "loan/payment/loanApprovePaymentExportlist",
            getTotal: "loan/payment/countLoanApprovePaymentExport",
            exportExcel: "loan/payment/exportLoanApprovePaymentExport",
            getGuarantee: "cooperation/guarantee/all",
            editPayPassWord:"loan/payment/editPayPassWord",
            getEditVeriCode:"loan/payment/getEditVeriCode",
            validatePayPassWordInfo:"loan/payment/validatePayPassWordInfo"
        },
        personal: {
            modifyPWD: "updPassWord"
        },
        flowControl: {
            getFlow: "flow/get",
            getNode: "flow/nodes",
            searchFlow: "rule/search",
            flowEnable: "rule/enable",
            flowDisable: "rule/disable",
            flowModify: "rule/update",
            flowDel: "rule/delete",
            flowSee: "rule/get",
            flowAdd: "rule/add",
            flowUpdate: "rule/updte",
            operator: "biz/user/list",
            operatorNew: "biz/userList",
            searchRulesGroup: "rule/searchRulesGroup",
            deleteRulesGroup: "rule/deleteRulesGroup",
            enableRulesGroup: "rule/enableRulesGroup",
            disableRulesGroup: "rule/disableRulesGroup",
            getRulesGroup: "rule/getRulesGroup",
            addRulesGroup: "rule/addRulesGroup",
            updateRulesGroup: "rule/updateRulesGroup",
            getUsersByFlowCompaniesBizGroups: "biz/getUsersByFlowCompaniesBizGroups"
        },
        printContract: {
            getList: "loanTemplateManage/loanTemplateList",
            getModalCode: "loanTemplateManage/loanTemplateContentList"
        },
        //待打款列表
        finance: {
            getList: 'finance/paymentapplication/list',
            getLists: 'finance/paymentapplication/lists',
            systempay: 'finance/paymentapplication/systempay',
            manpay: 'finance/paymentapplication/manpay',
            cancelpay: 'finance/paymentapplication/cancelpay',
            exportcustomer: 'finance/paymentapplication/exportcustomer',
            printbudget: 'finance/paymentapplication/printbudget',
            returnfund: 'finance/paymentapplication/returnfund',
            batchExportcustomer: 'finance/paymentapplication/exportcustomer/batch',
            batchManpay: 'finance/paymentapplication/manpay/batch',
            batchSystempay: 'finance/paymentapplication/systempay/batch',
            getCapatilPoolAccountList: 'cooperation/getCapatilPoolAccountList',
            guaranteeList: 'cooperation/guarantee/list',
            receiptList: 'finance/receipt/list',
            guaranteeList: 'cooperation/guarantee/list',
            guaranteeList: 'cooperation/guarantee/list',
            receiptList:"finance/receipt/list",
            receiptAmount:"finance/receipt/amount",
            //待打印预算单列表
            printapplicationList:"finance/printapplication/list",
            receiptPrint:"finance/receipt/print"
        },
        //数据看板
        dataView: {
            companyinfoList: 'ao/companyinfo/list',
            companyinfoGet: 'ao/companyinfo/get',
            loaninfoGet: 'loaninfo/getByProvince',
            loaninfoGetByStype: 'loaninfo/getByStype',
            distributionAmount: 'loaninfo/distribution/amount',
            distributionBusiness: 'loaninfo/distribution/business',
            loaninfoGetTop10: 'loaninfo/getTop10',
            settingsAll: 'backend/settings/all',
            generalUpdate: 'backend/settings/general/update',
            dataUpdate: 'backend/settings/data/update',
            overduepledgeGet: 'overdue/pledge/getByProvince',
            pledgeGetTop10: 'overdue/pledge/getTop10',
            pledgeGetAll: 'overdue/pledge/getAll',
            businessGetTop10: 'business/getTop10',
            insuranceGetAll: 'business/insurance/getAll',
            insuranceGetByStype: 'business/insurance/getByStype',
            creditcardGetAllByType: 'business/creditcard/getAllByType',
            creditcardGetByCategory: 'business/creditcard/getByCategory',
        },
        //保险分发（中安）
        distributionInsurance: {
            insureList: 'loanInsuranceIsSue/list0', //待分发
            isSue: 'loanInsuranceIsSue/update',
            insuranceList: 'loanInsuranceIsSue/list1'  //已分发
        },
        //保险分发（公司）
        distributionInsuranceCo: {
            insureCheck: 'loanInsuranceIsSue/insureCheck',
            insureSubmit: 'loanInsuranceIsSue/insureSubmit',
            export: 'loanInsuranceIsSue/export',
            export2: 'loanInsuranceIsSue/export2Selected',
            isSue: 'loanInsuranceIsSue/updatePolicyNumber',
            getInsure: "loanInsuranceIsSue/get",
            saveAll: 'loanInsuranceIsSue/add',
            preSubmit: "insurance/dispatch/process/preSubmit",
            submit2next: "insurance/dispatch/process/submit2next",
            cancel: "insurance/dispatch/process/cancel",
            back2pre: "insurance/dispatch/process/back2pre",
            exportCheck: "loanInsuranceIsSue/exportCheck",
            sendExport:"loanInsuranceIsSue/exportDispatch"//已分发导出
        },
        //贴息政策管理
        discountManage: {
            discountPolicySearch: "discount/policy/search",
            discountHostFacSelectList: "discount/policy/hostFacSelectList",
            discountPolicyCopy: "discount/policy/copy",
            discountPolicyDel: "discount/policy/del",
            discountPolicyChangeStatus: "discount/policy/changeStatus",
            discountPolicyBaseInfo: "discount/policy/show",
            discountBaseInfoSave: "discount/base/save",
            discountCarDealerList: "discount/carDealer/list",
            discountCarDealerImport: "discount/carDealer/import",
            discountCarDealerDownload: "discount/carDealer/download",
            discountCarDealerAdd: "discount/carDealer/add",
            discountCarDealerDel: "discount/carDealer/del",
            discountCarDealerChangeStatus: "discount/carDealer/changeStatus",
            discountCarDealerUpdate: "discount/carDealer/update",
            discountSchemeList: "discount/scheme/list",
            discountSchemeDel: "discount/scheme/del",
            discountSchemeCopy: "discount/scheme/copy",
            discountSchemeSetStatus: "discount/scheme/setStatus",
            discountSchemeSave: "discount/scheme/save",
            discountSchemeGet: "discount/scheme/get"
        },
        discountPolicy: {
            discountLoanGetBrand: "discount/loan/get2",
            discountLoanGetMake: "discount/loan/get3",
            discountLoanGetModel: "discount/loan/get4",
            discountLoanGetPolicy: "discount/loan/get5",
            discountLoanGetScheme: "discount/loan/get6",
            discountLoanGetCarDealer: "discount/loan/get7",
            discountLoanGetPolicy1: "discount/loan/get8"
        },
        bankSecurityRegion:{//银行备案区域管理接口地址
        	add:"bankSecurityRegion/add",
        	list:"bankSecurityRegion/list",
        	delete:"bankSecurityRegion/del"
        }
    };

//判断值是否改变,用法:在input上增加data-check="_字段名;提示信息",在获取旧值的接口里保存window[_字段名]的值,同一window内字段名不重复
$(document).on('blur', "[data-check]", function () {
    var check = $(this).data("check"),
        _key = check.split(";", 2)[0],
        _tip = check.split(";", 2)[1];
    var newValue = $(this).val();
    if (newValue !== "" && newValue !== window[_key]) {
        tip({
            content: _tip
        })
    }
});
//枚举
contractStatus = function (value, row, index) {
	var formatterStr = "";
	if(row.authPlatform != "" && row.authPlatform != undefined && row.authPlatform != null){
		if(value == "" || value == undefined || value == null || value == "0"){
			formatterStr = "未签";
		}
		if(value == "1"){
			formatterStr = "有效";
		}
		if(value == "2"){
			formatterStr = "失效";
		}
	}
	return formatterStr;
};

isLinkedLoan = function (value, row, index) {
	if(row.loanNum == "" || row.loanNum == undefined || row.loanNum == null){
		return "否";
	}else{
		return "是";
	}
};

statuss = function (value, row, index) {
    if (typeof value === "string") {
        value = parseInt(value);
    }
    return (value === 1 && "未生效") || (value === 2 && "已生效") || (value === -1 && "已删除") || "";
};

checkResult = function (value, row, index) {
    return ["待核对", "无误", "有误"][value] || null;
};

cardType = function (value, row, index) {
    return [null, "身份证", "军官证", "侨胞证", "外籍人士", ""][value] || null;
};

isAdvanceDiscount = function (value, row, index) {
    return [null, "是", "否"][value] || null;
};

resultStatus = function (value, row, index) {
    return [null, "未处理", "关闭", "退回", "发起征信", "已分配"][value] || null;
};

managementType = function (value, row, index) {
    return [null, "管理权", "业务权"][value] || null;
};
guaranteeType = function (value, row, index) {
    return [null, "抵押"][value] || null;
};

currencyType = function (value, row, index) {
    return [null, "抵押"][value] || null;
};
insuranceState = function (value, row, index) {
    return ["待投保", "投保中", "已投保"][value] || null;
}
estimateStatus = function (value, row, index) {
    return ["评估发起", "初评中", "初评完成", "评估中", "评估接收中","评估完成","已作废","已关闭","评估否决"][value] || null;
};

relationship = function (value, row, index) {
    return [null, "本人", "夫妻", "父亲", "母亲", "兄弟", "姐妹", "儿子", "亲戚", "朋友", "合伙人", "同事", "女儿", "姐夫", "嫂子", "儿媳"][value] || null;
};

relationshipWithLoaner = function (value, row, index) {
    return [null, "父母", "兄弟姐妹", "子女", "同事", "同学", "朋友"][value] || null;
};
mobileBase=function(value,row,index){
	return [null,"一致","不一致"][value]||null;
}
isNo = function (value, row, index) {
    return [null, "是", "否"][value] || null;
};

financialType = function (value, row, index) {
    return [null, "征信中", "征信通过", "未通过", "失效"][value] || null;
};

creditStatus = function (value, row, index) {
    return [null, "已征信", "未征信"][value] || null;
};

businessBreed = function (value, row, index) {
    return [null, "信用卡贷款", "银行直销-非垫款", "个人消费贷款"][value] || null;
};

level = function (value, row, index) {
    return [null, "一般", "紧急"][value] || null;
};

loanTerm = function (value, row, index) {
    return [null, "12期", "18期", "24期", "36期"][value] || null;
};

costType = function (value, row, index) {
    return [null, "公司", "车行", "客户"][value] || null;
};

number = function (value, row, index) {
    return ["元", "1", "2", "3"][value] || null;
};

feeType = function (value, row, index) {
    return [null, "无", "保险手续费", "补收印花税", "抵押押金（非现金）", "短期保费", "公证费（非现金)", "其他费用", "银行中介费（现金）", "银行手续费", "调查费"][value] || null;
};

deliveryMethod = function (value, row, index) {
    return [null, "现金", "非现金"][value] || null;
};

accountType = function (value, row, index) {
    return [null, "对公账户"][value] || null;
};

sex = function (value, row, index) {
    return ["女", "男"][value] || null;
};

maritalStatus = function (value, row, index) {
    return [null, "已婚", "未婚", "离异", "丧偶"][value] || null;
};

settleMethod = function (value, row, index) {
    return [null, "车行结算", "个人结算", "无需结算"][value] || null;
};

loanStatus = function (value, row, index) {
    return [null, "贷款拒绝", "审批通过", "贷款审核", "贷款发起", "贷款作废"][value] || null;
};

loanStatusQuery = function (value, row, index) {
    return [null, "业务办理中", "分公司审批中", "总部审批中", "审批通过", "审批否决", "贷款作废", "分公司请款中", "已付款未放款", "已付款已放款", "贷款修改中", "贷款作废中", "贷款结清", "收款中", "已收款", "未付款已放款"][value] || null;
};

deliverType = function (value, row, index) {
    return [null, "快递", "客户自取", "客户经理代送", "其他"][value] || null;
};

assetType = function (value, row, index) {
    return [null, "汽车", "房产", "地产", "银行存款", "其他", "机器设备类", "设施类在建工程", "有价证券", "保证金"][value] || null;
};

loanInfoYears = function (value, row, index) {
    return [null, "12个月", "18个月", "24个月", "36个月"][value] || null;
};

failReason = function (value, row, index) {
    return [null, "要示本人到场", "资料不齐全", "拍牌", "其他"][value] || null;
};

guarantyRelationship = function (value, row, index) {
    return [null, "担保人", "反担保人"][value] || null;
};

loanCarType = function (value, row, index) {
    return [null, "新车", "二手车"][value] || null;
};
//节点分流操作状态枚举
flow_status = function (value, row, index) {
    return ["停用", "启用"][value] || null;
};
//导出状态判断
export_Status = function (value, row, index) {
    return [null, "未导出", "已导出"][value] || null;
};
//专项卡开卡状态
var card_apply_status
card_apply_status = function (value, row, index) {
    return ["未开卡", "未开卡", "未开卡", "未开卡","开卡失败", "开卡成功"][value] || "未开卡";
};
yuan = function (value, row, index) {
    if (value) {
        return value + "元";
    } else {
        return null;
    }
};

approveStatus = function (value, row, index) {
    if (typeof value === "string") {
        value = parseInt(value);
    }
    return (value === 0 && "未审批") || (value === 1 && "审批中") || (value === 2 && "审批通过") || (value === 3 && "审批拒绝") || (value === 4 && "失效") || null;
};
var signStatus;
signStatus = function (value, row, index) {
    if (typeof value === "string") {
        value = parseInt(value);
    }
    return ["试单中","合作中","受限中"][value];
};
isDefault = function (value, row, index) {
    return (value ? "是" : "否");
}
carDealerStatus = function (value, row, index) {
    if (typeof value === "string") {
        value = parseInt(value);
    }
    return (value === 0 && "已停用") || (value === 1 && "已启用") || null;

};

cavStatus = function (value, row, index) {
    return [null, "已核销", "未核销", "已退单"][value] || null;
}
fileSended = function (value) {
    if (value != "") {
        var arrayValue = "";
        //从右到左：抵押合同、还款卡、抵押委托书、合同资料、抵押资料
        if ((value & 1) == 1)arrayValue = arrayValue + "抵押资料" + "、";
        if ((value & 2) == 2)arrayValue = arrayValue + "合同资料" + "、";
        if ((value & 4) == 4)arrayValue = arrayValue + "抵押委托书" + "、";
        if ((value & 8) == 8)arrayValue = arrayValue + "还款卡" + "、";
        if ((value & 16) == 16)arrayValue = arrayValue + "抵押合同" + "、";
        arrayValue = arrayValue.substring(0, arrayValue.length - 1);
        return arrayValue;
    } else {
        return "-";
    }
};
recipientStatus = function (value) {
    switch (value) {
        case 1:
            return "未收件";
            break;
        case 2:
            return "资料缺失";
            break;
        case 3:
            return "已收件";
    }
};

assetsPackageStatus = function (value, row, index) {
    if (typeof value === "string") {
        value = parseInt(value);
    }
    return (value === 1 && "生效中") || (value === 2 && "过期") || null;
};

dateFormTen = function (value, row, index) {
    if (value && value.length > 10) {
        return value.substr(0, 10);
    } else {
        return value;
    }
};

gpsType = function (value) {
    return [null, "有线", "无线", "有线+无线"][value] || null;
};


//当前月，年
var date, year, month, nowMonth;
date = new Date();
year = date.getFullYear();
month = date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
nowMonth = year + "-" + month;

// 日期格式化插件
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    };
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1,
                RegExp.$1.length == 1 ? o[k] :
                    ("00" + o[k]).substr(("" + o[k]).length));
    return format;
};

//确认提交或退回模态框
var sureModal = '<div class="modal fade" id="sureModal">' +
    '<div class="modal-dialog">' +
    '<div class="modal-content">' +
    '<div class="modal-header">' +
    '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
    '<h4 class="modal-title">提示信息</h4>' +
    '</div>' +
    '<div class="modal-body">' +
    '<p class="tipText"></p>' +
    '</div>' +
    '<div class="modal-footer">' +
    '<button type="button" class="btn btn-primary" id="sureOption">确定</button>' +
    '<button type="button" class="btn btn-default" data-dismiss="modal">取消</button>' +
    '</div></div></div></div>';

function oppSureModal(text) {
    if ($("#sureModal").length > 0) {
        $("#sureModal").modal("show");
        $("#sureModal").find(".tipText").text(text);
    } else {
        $("body").append(sureModal);
        $("#sureModal").find(".tipText").text(text);
        $("#sureModal").modal("show");
    }
}

$.fn.extend({
    getBank: function () {
        comn.ajax({
            url: interUrl.gr.bankList,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.bankName + "</option>");
                            }
                            return results;
                        })()).join(""));
                };
            })(this)
        });
        return this;
    },
    getUserByCompanyId: function (companyId) {
        if (companyId) {
            comn.ajax({
                url: interUrl.common.getUserByCompanyId,
                data: {
                    companyId: companyId
                },
                success: (function (_this) {
                    return function (res) {
                        var j, len, o, ref, str;
                        str = "<option value=''>--请选择--</option>";
                        var defaultValue = $(_this).attr('defaultValue');
                        ref = res.data;
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            str += "<option value='" + o.uid + "' " + (defaultValue == o.uid ? "selected" : "") + ">" + o.realname + "</option>";
                        }
                        return $(_this).html(str);
                    };
                })(this)
            });
            return this;
        }
    },
    getGroup: function (companyId, value) {
        if (companyId) {
            comn.ajax({
                url: interUrl.common.getGroup,
                data: {
                    companyId: companyId
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                                var j, len, ref, results;
                                ref = res.data;
                                results = [];
                                for (j = 0, len = ref.length; j < len; j++) {
                                    o = ref[j];
                                    results.push("<option value='" + o.id + "'>" + o.name + "</option>");
                                }
                                return results;
                            })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getLoanApplyBankList: function (customerId,value) {  //客户征信银行列表
        if(customerId) {
            comn.ajax({
                url: interUrl.common.loanApplyBankList,
                data:{customerId:customerId},
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                                var j, len, ref, results;
                                ref = res.data;
                                results = [];
                                for (j = 0, len = ref.length; j < len; j++) {
                                    o = ref[j];
                                    results.push("<option data-isdiscount='" + o.discount + "' value='" + o.inquryBankId + "'>" + o.inquryBank + "</option>");
                                }
                                return results;
                            })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getDealerName: function (value, data) {
        comn.ajax({
            url: interUrl.carDealer.selectList,
            data: {
                dealerId: data
            },
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.dealerName + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    flowGet: function (value) {
        comn.ajax({
            url: interUrl.common.flowGet,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.flowType + "'>" + o.flowName + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getFlowNode: function (flowName, value) {
        if (flowName) {
            comn.ajax({
                url: interUrl.flowControl.getNode,
                data: {
                    businessTypeCode: flowName
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                                var j, len, ref, results;
                                ref = res.data;
                                results = [];
                                for (j = 0, len = ref.length; j < len; j++) {
                                    o = ref[j];
                                    results.push("<option value='" + o.nodeCode + "'>" + o.nodeName + "</option>");
                                }
                                return results;
                            })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getFlowOperator: function (groupId, value) {
        if (flowName) {
            comn.ajax({
                url: interUrl.flowControl.operator,
                data: {
                    bizGroupId: groupId
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                                var j, len, ref, results;
                                ref = res.data;
                                results = [];
                                for (j = 0, len = ref.length; j < len; j++) {
                                    o = ref[j];
                                    results.push("<option value='" + o.uid + "'>" + o.realname + "</option>");
                                }
                                return results;
                            })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getFlowOperator_new: function (groupId, value) {
        if (groupId) {
            comn.ajax({
                url: interUrl.flowControl.operatorNew,
                data: groupId,
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                                var j, len, ref, results;
                                ref = res.data;
                                results = [];
                                for (j = 0, len = ref.length; j < len; j++) {
                                    o = ref[j];
                                    results.push("<option value='" + o.uid + "'>" + o.realname + "</option>");
                                }
                                return results;
                            })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getFlowOperatorByCompaniesBizGroups: function (groupId, value) {
        if (groupId) {
            comn.ajax({
                url: interUrl.flowControl.getUsersByFlowCompaniesBizGroups,
                data: groupId,
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.uid + "'>" + o.realname + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    //获取资金部待打款列表合作银行下拉列表
    bank_Get: function (value) {
        comn.ajax({
            url: interUrl.export.getBank,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.bankName + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    //报表查询-银行放款情况分析页，合作银行下拉列表(获取启用、停用的所有银行)
    bank_GetAllWithoutStatus: function (value) {
        comn.ajax({
            url: interUrl.export.getAllWithoutStatus,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.bankName + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    //获取资金部待打款列表收款人全称下拉列表
    getGuarantee_Get: function (value) {
        comn.ajax({
            url: interUrl.export.getGuarantee,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.organizationName + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getExpressCompanyCode: function (type) {
        //type : "ExpressCompany" 快递公司(默认)
        //type : "RelationShipType" 关系人
        comn.ajax({
            url: interUrl.gr.expressCompanyCode,
            data: {codeType: type || 'ExpressCompany'},
            success: (function (_this) {
            	
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            console.log(ref);
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.codeId + "'>" + o.codeName + "</option>");
                            }
                            return results;
                        })()).join(""));
                };
            })(this)
        });
        return this;
    },
  //=====================业务录入获取关系人列表,剔除”本人“====================再次提交
    
    getRelationShipTypeCode: function (type) {
        //type : "RelationShipType" 关系人
        comn.ajax({
            url: interUrl.gr.expressCompanyCode,
            data: {codeType: type || 'RelationShipType'},
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                if(o.codeName == '本人' || o.codeName == '夫妻'){//紧急联系人“关系人”字段删去“夫妻”选项
                                	continue;
                                }
                                results.push("<option value='" + o.codeId + "'>" + o.codeName + "</option>");
                            }
                            return results;
                        })()).join(""));
                };
            })(this)
        });
        return this;
    },
  
   
    // 逾期数据导入银行插件
    getBankAll: function () {
        comn.ajax({
            url: interUrl.gr.getBankAll,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.bankName + "</option>");
                            }
                            return results;
                        })()).join(""));
                };
            })(this)
        });
        return this;
    },
    getPolicy: function (_data, value,name) { //获取贴息政策
        //参数说明:coId:银行id ,carBrand:品牌code  carMake:车系code   carModel:车型code,  value:当前的code  name:当前的option的name
        comn.ajax({
            url: interUrl.discountPolicy.discountLoanGetPolicy,
            data: _data,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results,codeArr=[];
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                codeArr.push(o.id);
                                results.push("<option value='" + o.id + "'>" + o.policyName + "</option>");
                            }
                            if($.inArray(value,codeArr)==-1 && value && name){ //如果列表项被停用,则手动加入
                                results.push("<option value='" + value + "'>" + name + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getScheme: function (_data, value, name, disPolicySchemeNpers) { //获取贴息方案
        //参数说明:policyId:贴息政策policyId ,carBrand:品牌code  carMake:车系code  carModel:车型code  nper:期数  value:当前的code  name:当前的option的name  disPolicySchemeNpers:当前方案的公式
        if (_data) {
            comn.ajax({
                url: interUrl.discountPolicy.discountLoanGetScheme,
                data: _data,
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                                var j, len, ref, results,codeArr=[];
                                ref = res.data;
                                results = [];
                                for (j = 0, len = ref.length; j < len; j++) {
                                    o = ref[j];
                                    codeArr.push(o.id);
                                    var formula = JSON.stringify(o.disPolicySchemeNpers[0]); //计算贴息金额需要存的数据
                                    results.push("<option value='" + o.id + "' data-formula='" + formula + "'>" + o.schemeName + "</option>");
                                }
                                if($.inArray(value,codeArr)==-1 && value && name && disPolicySchemeNpers){ //如果列表项被停用,则手动加入
                                    var _disPolicySchemeNpers=JSON.stringify(disPolicySchemeNpers);
                                    results.push("<option value='" + value + "' data-formula='" + _disPolicySchemeNpers + "'>" + name + "</option>");
                                }
                                return results;
                            })()).join("")).val(value || "");
                    };
                })(this)
            });
            return this;
        }
    },
    getOpeningBank: function () {
        comn.ajax({
            url: interUrl.gr.getOpeningBank,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.bankName + "</option>");
                            }
                            return results;
                        })()).join(""));
                };
            })(this)
        });
        return this;
    },
    // 逾期数据导入银行模版插件
    getTemplateList: function (coBankId, value) {
        if (coBankId) {
            comn.ajax({
                url: interUrl.gr.getTemplateList,
                data: {
                    coBankId: coBankId
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                                var j, len, ref, results;
                                ref = res.data;
                                results = [];
                                for (j = 0, len = ref.length; j < len; j++) {
                                    o = ref[j];
                                    results.push("<option value='" + o.id + "'>" + o.name + "</option>");
                                }
                                return results;
                            })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getFinancialProduct: function (loanTerm, coBankId, businessTypeId, value) {
        if (coBankId && businessTypeId) {
            comn.ajax({
                url: interUrl.loanDetail.getFinancialProduct,
                data: {
                    loanTerm: loanTerm,
                    coBankId: coBankId,
                    businessTypeId: businessTypeId
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                                var j, len, ref, results;
                                ref = res.data;
                                results = [];
                                for (j = 0, len = ref.length; j < len; j++) {
                                    o = ref[j];
                                    results.push("<option value='" + o.id + "' data-nper='" + o.nper + "'>" + o.productName + "</option>");
                                }
                                return results;
                            })()).join("")).val(value || "");
                    };
                })(this)
            });
            return this;
        }
    },
    getFinancialPlan : function (value) {
    	comn.ajax({
    		url: interUrl.gr.financialPlanList,
    		success: (function (_this) {
    			return function (res) {
    				var o;
    				return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
    					var j, len, ref, results;
    					ref = res.data;
    					results = [];
    					for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
    						results.push("<option value='" + o.id + "'>" + o.planName + "</option>");
    						}
    					return results;
    				})()).join("")).val(value || "");
    			};
    		})(this)
    	});
      return this;
    },
    getOccupationList: function (value) {
        comn.ajax({
            url: interUrl.myTask.occupationList,
            data: {
                codeType: 'Profession'
            },
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.codeId + "'>" + o.codeName + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getJobList: function (value) {
        comn.ajax({
            url: interUrl.myTask.jobList,
            data: {
                codeType: "Post"
            },
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.codeId + "'>" + o.codeName + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getUnitList: function (value) {
        comn.ajax({
            url: interUrl.myTask.unitList,
            data: {
                codeType: 'WorkNature'
            },
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.codeId + "'>" + o.codeName + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getToday: function () {
        var date, y, m, d, today;
        date = new Date();
        y = date.getFullYear();
        m = date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        d = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        today = y + "-" + m + "-" + d;
        $(this).val(today);
    },
    getMonthDay1: function () {
        var date, y, m, d, today;
        date = new Date();
        y = date.getFullYear();
        m = date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        today = y + "-" + m + "-" + "01";
        $(this).val(today);
    },
    getLastMonthDay1: function () {
        var date, y, m, d, today;
        date = new Date();
        y = date.getFullYear();
        m = date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        today = y + "-" + m + "-" + "01";
        $(this).val(today);
    },
    getYear1Day1: function () {
        var date, y, m, d, today;
        date = new Date();
        y = date.getFullYear();
        m = date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        d = date.getDate();
        today = (y + 1) + "-" + m + "-" + (d - 1);
        $(this).val(today);
    },
    //当年第一月
    getMonthFirst: function () {
        var date = new Date();
        var MonthFirst = date.getFullYear();
        $(this).val(MonthFirst + "-01");
    },
    //当月
    getMonthCur: function () {
        var date = new Date();
        var currentMonth = date.getMonth();
        var MonthFirstDay = new Date(date.getFullYear(), currentMonth, 1).format('yyyy-MM');
        $(this).val(MonthFirstDay);
    },
    //当月第一天
    getMonthDayFirst: function () {
        var date = new Date();
        var currentMonth = date.getMonth();
        var MonthFirstDay = new Date(date.getFullYear(), currentMonth, 1).format('yyyy-MM-dd');
        $(this).val(MonthFirstDay);
    },
    //当月最后一天
    getMonthDayLast: function () {
        var date = new Date();
        var currentMonth = date.getMonth();
        var nextMonth = ++currentMonth;
        var nextMonthFirstDay = new Date(date.getFullYear(), nextMonth, 1);
        var oneDay = 1000 * 60 * 60 * 24;
        var today = new Date(nextMonthFirstDay - oneDay).format('yyyy-MM-dd');
        $(this).val(today);
    },
    getLoanFlowStatusList: function () {
        comn.ajax({
            url: interUrl.common.getLoanFlowStatusList,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data.flowStatus;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.code + "'>" + o.name + "</option>");
                            }
                            return results;
                        })()).join(""));
                };
            })(this)
        });
        return this;
    },
    //获取流程意见
    getOpinion_s: function (data) {
        comn.ajax({
            url: interUrl.common.getOpinion,
            data: data,
            success: (function (_this) {
                return function (res) {
                    $(_this).val(res.data);
                };
            })(this)
        });
    },
    //获取合作银行
    getCooperationUnit: function () {
        comn.ajax({
            url: interUrl.common.cooperation,
            success: (function (_this) {
                return function (res) {
                    console.log(res.data);
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.codeId + "'>" + o.codeName + "</option>");
                            }
                            return results;
                        })()).join(""));
                };
            })(this)
        });
        return this;
    }
});

function score(a) {    //评分展示
    var this_ = $(".score");
    if (type == 1 || type == 2 || type == 3) {
        this_.children('.scoreNum').html(a);
        if (a >= 101) {
            this_.attr("fors", "03").show();
        } else if (a >= 81 && a <= 100) {
            this_.attr("fors", "02").show();
        } else if (a < 81) {
            this_.attr("fors", "01").show();
        }
    }
}

//将通融单类型转换为通融单名称
function getAccommodateTypeNameByValue(accommodateType) {
    var accommodateTypeName = '';
    switch (accommodateType) {
    case '1':
        accommodateTypeName = "单签";
        break;
    case '2':
        accommodateTypeName = "拆单";
        break;
    case '3':
        accommodateTypeName = "免担保";
        break;
    case '4':
        accommodateTypeName = "特殊通融";
        break;
    }
    return accommodateTypeName;
}

//比较时间大小
function compareDate(d1,d2,flag,duration) {
    if (flag == true){
        var dd1 = new Date(d1);
        dd1.setMonth(dd1.getMonth()+duration);
        return  new Date(dd1) < (new Date(d2));
    }else{
        return  (new Date(d1)) > (new Date(d2));
    }
}

//强制保留两位小数
function toDecimal2(x) {
    var f = parseFloat(x);
    if (isNaN(f)) {
        return false;
    }
    var f = Math.round(x*100)/100;
    var s = f.toString();
    var rs = s.indexOf('.');
    if (rs < 0) {
        rs = s.length;
        s += '.';
    }
    while (s.length <= rs + 2) {
        s += '0';
    }
    return s;
}
