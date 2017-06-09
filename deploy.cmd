@echo off

    echo deploying the API ...

    call deploy.api.cmd
    
    echo deploying the web site....

    call deploy.app.cmd

    
