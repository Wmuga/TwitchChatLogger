let messages = {}
let channels = []
let currentChannel = ''
let timestamp = -1
const maxMessages = 30
let intervalId = -1
const msgReg = /("timestamp":)(.+?)(,|})/g
function makeRequest(url, method = "GET", callback = undefined) {
	return function () {
		fetch(url, {method})
			.then(res => {
				if (callback)
					return res.text()
			})
			.then(bd => {
				if (callback) {
					bd = bd.replaceAll(msgReg, '$1"$2"$3')
					callback(JSON.parse(bd))
				}
			})
	}
}


const loadSettings = makeRequest('/settings', "GET", (res) => {
	console.log(res);
	({ messages, channels, timestamp, currentChannel } = res);
	timestamp = BigInt(timestamp)
	setupInterval()
})

function getChannel() {
	return document.getElementById('channelName').value
}

function updateButtons() {
	let btnsContainer = document.getElementById('channels-buttons')
	btnsContainer.innerHTML = ''

	for (let channel of channels) {
		let btn = document.createElement('button')
		btn.innerText = channel
		btn.onclick = () => channelButtonClick(channel)
		btnsContainer.appendChild(btn)
	}
}

function channelButtonClick(channel) {
	currentChannel = channel
	setupInterval()
}

function addChannel() {
	let channel = getChannel()
	if (channel.length == 0) return
	channels.push(channel[0]!='#' ? '#'+channel : channel)
	updateButtons()
	if (channel[0] == '#') channel = channel.slice(1)
	const req = makeRequest('/Listen/Add?channel=' + channel, "POST")
	req()
}

function removeChannel() {
	let channel = getChannel()
	if (channel.length == 0) return
	channels = channels.filter(e => e != (channel[0] != '#' ? '#' + channel : channel))
	updateButtons()
	if (channel[0]=='#') channel = channel.slice(1)
	const req = makeRequest('/Listen/Remove?channel=' + channel, "DELETE")
	req()
}

function getMessages(channel, callback) {
	if (!channel || !(channels.find((a) => a == currentChannel))) return
	channelQuery = channel
	if (channel[0] == '#') channelQuery = channel.slice(1)
	const req = makeRequest(`/Messages?channels=${channelQuery}&timestamp=${timestamp}`, 'GET', (res) => {
		if (channel[0] != '#') channel = '#' + channel
		let temp = [...messages[channel], ...res[channel]]
		if (temp.length > maxMessages) {
			temp = temp.splice(temp.length - maxMessages+1)
		}
		messages[channel] = temp
		for (let msg of temp) {
			timestamp = timestamp < msg.timestamp? BigInt(msg.timestamp) : timestamp
		}
		callback()
	})
	req()
}

function setupInterval() {
	if (intervalId != -1) clearInterval(intervalId)
	selectChannel(currentChannel)
	intervalId = setInterval(() => { selectChannel(currentChannel) }, 1000)
}

function selectChannel(channel) {
	if (!messages[channel]) messages[channel] = []
	getMessages(channel,() => {
		let chatTable = document.getElementById("chatbox").children[0]

		chatTable.innerHTML = ''

		for (let msg of messages[channel]) {
			let tr = document.createElement("tr")
			tr.classList.add("message")
			let user = document.createElement("td")
			user.classList.add("user")
			user.style.color = msg.color;
			user.innerText = msg.user;
			let content = document.createElement("td")
			content.classList.add("content")
			content.innerText = msg.content;
			tr.appendChild(user)
			tr.appendChild(content)
			chatTable.appendChild(tr)
		}
	})
}


window.addEventListener('load', () => loadSettings())
