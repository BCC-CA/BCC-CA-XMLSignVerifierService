# BCC-CA-XMLSignVerifierService

## Deployment Tutorial can be found in [here](https://www.vultr.com/docs/how-to-deploy-a-net-core-web-application-on-centos-7) and [here](https://docs.microsoft.com/en-us/dotnet/core/install/linux-centos).

## Demo Commands For deployment Project-

    sudo dnf install dotnet-sdk-3.1 aspnetcore-runtime-3.1 dotnet-runtime-3.1 supervisor -y && sudo vim /etc/supervisord.d/xml_sign_verifier_service.conf

Now write this and save the doc with `wq!`-

    [program:xml_sign_verifier_service]
    command=dotnet SinedXmlVelidator.dll --urls "http://*:5050"
    directory=/home/<your_user_name>/PublishedXmlVerifireService/
    environment=ASPNETCORE__ENVIRONMENT=Production
    user=root
    stopsignal=INT
    autostart=true
    autorestart=true
    startsecs=1
    stderr_logfile=/var/log/xml_sign_verifier_service.err.log
    stdout_logfile=/var/log/xml_sign_verifier_service.out.log

Now, Update `supervisor` service settings like this-

    sudo cp /etc/supervisord.conf /etc/supervisord.conf.bak && sudo vi /etc/supervisord.conf 

Find the last line:

    files = supervisord.d/*.ini

Replace it:

    files = supervisord.d/*.conf

Save and quit:

    :wq!

And run this command to make the app files-

    cd ~ && git clone https://github.com/AbrarJahin/BCC-CA-XMLSignVerifierService.git && sudo systemctl stop supervisord && rm -rf ~/PublishedXmlVerifireService && cd BCC-CA-XMLSignVerifierService && dotnet publish -c Release -o ~/PublishedXmlVerifireService && cd ~
    sudo systemctl start supervisord && cd ~ && tail -f /var/log/xml_sign_verifier_service.out.log

Now make the service up and running-

    sudo systemctl start supervisord.service && sudo systemctl enable supervisord.service && sudo supervisorctl reread && sudo supervisorctl update
    sudo supervisorctl status

## Optional-

Now allow the `5050` port from `iptable`, `network`, `firewall` and `selinux`.

## Demo Commands For Update Project-

    cd /home/abrar/XML-Signer-ASP.NetCore-PostGRE && git pull && sudo systemctl stop supervisord && rm -rf /home/abrar/PublishedWebApp && dotnet publish -c Release -o /home/abrar/PublishedWebApp && sudo systemctl start supervisord && cd ~ && tail -f /var/log/xml_sign_verifier_service.out.log

