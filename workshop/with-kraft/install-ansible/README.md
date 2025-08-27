# Install Kafka cluster wuth [Ansible](https://github.com/ansible/ansible)

## 1. Ansible requires [Python 3](https://www.python.org)
```
$pip install ansible
$pip install ansible-core

$ansible --version
$ansible-playbook --version
```

## 2. Details in Ansible playbooks for Kafka cluster
* 2 Nodes (required setup)
  * SSH's user to install

## 3. Setup all softwares
| File | Description | Detail |
|---------|-----|-----|
| hosts | Root User Deployment | broker1, broker2 |
| vars.yml | Variables file | broker1, broker2 |
| install.yml| Main file |

Try to setup in 2 nodes
* Java 17
* Kafka 4.0.0
```
$sudo adduser kafka
$sudo mkdir /opt/kafka
$sudo chown -R kafka:kafka /opt/kafka
$sudo chmod 755 /opt/kafka
```

Run
```
$cd play-books
$ansible-playbook -i hosts install.yml
```

## 4. Start Kafka cluster
* Change ip/server in `config/broker-n/server.properties`

Generate uuid of cluster id
```
$bin/kafka-storage.sh random-uuid
```

4.1 Update config
* Push config of kafka to all servers
* Initial log directory

```
$ansible-playbook -i hosts push-config.yml
```

4.2 Start cluster
```
$ansible-playbook -i hosts start.yml
```

4.3 Check status
```
$ansible-playbook -i hosts status.yml
```

4.4 Stop
```
$ansible-playbook -i hosts stop.yml
```

4.5 Restart
```
$ansible-playbook -i hosts restart.yml
```

4.6 Restart
```
$ansible-playbook -i hosts uninstall.yml
```

## 5. Connect to cluster
```
$docker compose up -d
```

Access to Kafka UI
* http://localhost:8080