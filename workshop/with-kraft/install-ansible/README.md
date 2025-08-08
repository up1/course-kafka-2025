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
  * JDK 17


## 3. Setup Deployment with non-root
| File | Description | Detail |
|---------|-----|-----|
| hosts | Root User Deployment | broker1, broker2 |
| vars.yml | Variables file | broker1, broker2 |
| install.yml| Main file |

Try to setup in 2 nodes
```
$sudo mkdir /opt/kafka
$sudo chown -R kafka:kafka /opt/kafka
$sudo chmod 755 /opt/kafka
```

Run
```
$ansible-playbook -i hosts install.yml
```
