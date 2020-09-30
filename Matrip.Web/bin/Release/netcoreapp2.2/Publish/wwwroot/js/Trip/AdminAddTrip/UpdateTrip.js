function addDiscount(tripID) {
    if (typeof tripID === 'undefined') {
        tripID = "";
    }
    var discountListDiv = document.getElementById("discountListDiv");
    var dicountDiv = document.getElementsByClassName("dicountDiv");
    var index = 0;
    if (dicountDiv != null) {
        index = dicountDiv.length;
    }
    const div = document.createElement("div");
    var discountNumber = index + 1;
    div.className = "dicountDiv";
    div.innerHTML = `<h6>Desconto ` + discountNumber + `</h6>
                <div>
                    <input hidden type="text" class="form-control" name="ma27AgeDiscountList[`+ index + `].FK2705idTrip" value="` + tripID +`" readonly required />
                </div>
                <div>
                    <label>Nome do Desconto (Obrigatório)</label>
                    <input type="text" class="form-control" name="ma27AgeDiscountList[`+ index + `].ma27name" required />
                </div>
                <div>
                    <label>Porcentagem de Desconto (Obrigatório)</label>
                    <input type="number" class="form-control" name="ma27AgeDiscountList[`+ index + `].ma27DiscountPercent" required />
                </div>
                <div>
                    <label>Idade Mínima (Padrão 0)</label>
                    <input type="number" class="form-control" name="ma27AgeDiscountList[`+ index + `].ma27minage" />
                </div>
                <div>
                    <label>Idade Máxima (Opcional)</label>
                    <input type="number" class="form-control" name="ma27AgeDiscountList[`+ index + `].ma27maxage" />
                </div>
                <div>
                    <label>Quantidade Mínima de Pessoas (Obrigatório)</label>
                    <input type="number" class="form-control" name="ma27AgeDiscountList[`+ index + `].ma27minPeople" required />
                </div>
                    <hr />`;
    discountListDiv.appendChild(div);
}

function addSubtrip(tripID) {
    if (typeof tripID === 'undefined') {
        tripID = "";
    }
    var subtripListDiv = document.getElementById("subtripListDiv");
    var subtripDiv = document.getElementsByClassName("subtripDiv");
    var index = 0;
    var space = "";
    if (subtripDiv != null) {
        index = subtripDiv.length;
        //space = space + " mt-5";
    }
    const div = document.createElement("div");
    var subtripNumber = index + 1;
    div.className = "row subtripDiv card-deck" + space;
    div.innerHTML = `
                <div class="col-3 ">
                    <h6>Passeio `+ subtripNumber + `</h6>
                    <div>
                        <input hidden type="text" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.FK1405idtrip" value="` + tripID +`" readonly required/>
                    </div>
                    <div>
                        <label>Nome do Passeio</label>
                        <input type="text" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14name" />
                    </div>
                    <div>
                        <label>Endereço do Passeio</label>
                        <input type="text" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14address" required />
                    </div>
                    <div>
                        <label>Bairro do Passeio</label>
                        <input type="text" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14neighborhood" required />
                    </div>
                    <div>
                        <label>Vaga Por Passeio</label>
                        <input type="number" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14vacancy" required />
                    </div>
                    <div>
                        <label>Quantidade de Grupos Por Passeio</label>
                        <input type="number" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14groupnumber" required />
                    </div>
                    <div>
                        <label>Descrição do Passeio</label>
                        <textarea class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14Description" required ></textarea>
                    </div>
                </div>
                <div class="col-3">
                    <div id="serviceListDiv:Subtrip-`+ index + `">
                        <a class="btn btn-outline-success" onclick="addService(`+ index + `);">Criar Serviço</a>
                        <hr />
                        <div class="serviceDiv:Subtrip-`+ index + `">
                                <h6>Serviço 1</h6>
                                <div>
                                    <label>Nome do Serviço</label>
                                    <input type="text" class="form-control" name="SubtripModelList[`+ index + `].ma11serviceList[0].ma11name" required />
                                </div>
                                <div>
                                    <label>Descrição do Serviço</label>
                                    <textarea class="form-control" name="SubtripModelList[`+ index + `].ma11serviceList[0].ma11description" required></textarea>
                                </div>
                                <div>
                                    <label>Quantidade Mínima</label>
                                    <input type="number" class="form-control" name="SubtripModelList[`+ index + `].ma11serviceList[0].ma11minQuantity" required />
                                </div>
                                <div>
                                    <label>Valor da Unidade</label>
                                    <input class="form-control" name="SubtripModelList[`+ index + `].ma11serviceList[0].ma11Value" onKeyPress="return(MascaraMoeda(this,'.',',',event))" required />
                                </div>
                                <hr />
                            </div>
                    </div>
                </div>
                <div class="col-3">
                    <div id="subtripValueListDiv:Subtrip-`+ index + `">
                        <a class="btn btn-outline-success" onclick="addSubtripValue(`+ index + `);">Criar Valor</a>
                        <hr />
                        <div class="subtripValueDiv:Subtrip-`+ index + `">
                            <h6>Valor 1</h6>
                            <div>
                                <label>Valor</label>
                                <input class="form-control" name="SubtripModelList[`+ index + `].ma17SubtripValueList[0].ma17value" onKeyPress="return(MascaraMoeda(this,'.',',',event))" required />
                            </div>
                            <div>
                                <label>Descrição do Valor</label>
                                <textarea class="form-control" name="SubtripModelList[`+ index + `].ma17SubtripValueList[0].ma17description" required></textarea>
                            </div>
                            <div>
                                <label>Status do Valor</label>
                                <select class="form-control" name="SubtripModelList[`+ index + `].ma17SubtripValueList[0].ma17status" required>
                                    <option value="0">Individual</option>
                                    <option value="1">Privativo</option>
                                    <option value="2">Pacote</option>
                                </select>
                            </div>
                            <div>
                                <label>Quantidade de Pessoas (se for pacote***)</label>
                                <input class="form-control" name="SubtripModelList[`+ index + `].ma17SubtripValueList[0].ma17quantity" />
                            </div>
                            <hr />
                        </div>
                    </div>
                </div>
                <div class="col-3">
                    <div id="subtripScheduleListDiv:Subtrip-`+ index + `">
                        <a class="btn btn-outline-success" onclick="addSubtripSchedule(`+ index + `);">Criar Horário</a>
                        <hr />
                        <div class="subtripScheduleDiv:Subtrip-`+ index + `">
                            <h6>Horário 1</h6>
                            <div>
                                <label>Dias do Horário</label>
                                <input class="form-control" type="text" name="SubtripModelList[`+ index + `].ma16subtripscheduleList[0].ma16days" required />
                            </div>
                            <div>
                                <label>Duração do Horário</label>
                                <input class="form-control" type="time" name="SubtripModelList[`+ index + `].ma16subtripscheduleList[0].ma16duration" required />
                            </div>
                            <div>
                                <label>Horário de Entrada</label>
                                <input class="form-control" type="time" name="SubtripModelList[`+ index + `].ma16subtripscheduleList[0].ma16entry" required />
                            </div>
                            <div>
                                <label>Horário de Saída</label>
                                <input class="form-control" type="time" name="SubtripModelList[`+ index + `].ma16subtripscheduleList[0].ma16exit" />
                            </div>
                            <hr />
                        </div>
                    </div>
                </div>
                    

                <hr/>`;
    subtripListDiv.appendChild(div);
}

function addService(subtripPosition, subtripID) {
    if (typeof subtripID === 'undefined') {
        subtripID = "";
    }
    var serviceListDiv = document.getElementById("serviceListDiv:Subtrip-" + subtripPosition);
    var serviceDiv = document.getElementsByClassName("serviceDiv:Subtrip-" + subtripPosition);
    var index = 0;
    var space = "";
    if (serviceDiv != null) {
        index = serviceDiv.length;
        space = "";
    }
    const div = document.createElement("div");
    div.className = "serviceDiv:Subtrip-" + subtripPosition + space;
    var serviceNumber = index + 1;
    div.innerHTML = `
                            
                            <h6>Serviço `+ serviceNumber + `</h6>
                            <input hidden type="text" class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma11serviceList[` + index + `].FK1114idsubtrip" value="` + subtripID+`" readonly required />
                                        
                            <div>
                                <label>Nome do Serviço</label>
                                <input type="text" class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma11serviceList[` + index + `].ma11name" required />
                            </div>
                            <div>
                                <label>Descrição do Serviço</label>
                                <textarea class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma11serviceList[` + index + `].ma11description" required ></textarea>
                            </div>
                            <div>
                                <label>Quantidade Mínima</label>
                                <input type="number" class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma11serviceList[` + index + `].ma11minQuantity" required />
                            </div>
                            <div>
                                <label>Valor da Unidade</label>
                                <input class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma11serviceList[` + index + `].ma11Value" onKeyPress="return(MascaraMoeda(this,'.',',',event))" required />
                            </div>
                            <hr />
                    `;
    serviceListDiv.appendChild(div);
}
function addSubtripValue(subtripPosition, subtripID) {
    if (typeof subtripID === 'undefined') {
        subtripID = "";
    }
    var subtripValueListDiv = document.getElementById("subtripValueListDiv:Subtrip-" + subtripPosition);
    var subtripValueDiv = document.getElementsByClassName("subtripValueDiv:Subtrip-" + subtripPosition);
    var index = 0;
    var space = "";
    if (subtripValueDiv != null) {
        index = subtripValueDiv.length;
        space = "";
    }
    const div = document.createElement("div");
    div.className = "subtripValueDiv:Subtrip-" + subtripPosition + space;
    var valueNumber = index + 1;
    div.innerHTML = `
                           
                    <h6>Valor `+ valueNumber + `</h6>
                    <div>
                        <input hidden class="form-control" asp-for="SubtripModelList[`+ subtripPosition + `].ma17SubtripValueList[` + index + `].FK1714idsubtrip" value="` + subtripID + `" required />
                    </div>
                    <div>
                        <label>Valor</label>
                        <input class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma17SubtripValueList[` + index + `].ma17value" onKeyPress="return(MascaraMoeda(this,'.',',',event))" required />
                    </div>
                    <div>
                        <label>Descrição do Valor</label>
                        <textarea class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma17SubtripValueList[` + index + `].ma17description" required></textarea>
                    </div>
                    <div>
                        <label>Status do Valor</label>
                        <select class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma17SubtripValueList[` + index + `].ma17status" required>
                            <option value="0">Individual</option>
                            <option value="1">Privativo</option>
                            <option value="2">Pacote</option>
                        </select>
                    </div>
                    <div>
                        <label>Quantidade de Pessoas (se for pacote***)</label>
                        <input class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma17SubtripValueList[` + index + `].ma17quantity" />
                    </div>
                          <hr />  
                    `;
    subtripValueListDiv.appendChild(div);
}

function addSubtripSchedule(subtripPosition, subtripID) {
    if (typeof subtripID === 'undefined') {
        subtripID = "";
    }
    var subtripScheduleListDiv = document.getElementById("subtripScheduleListDiv:Subtrip-" + subtripPosition);
    var subtripScheduleDiv = document.getElementsByClassName("subtripScheduleDiv:Subtrip-" + subtripPosition);
    var index = 0;
    var space = "";
    if (subtripScheduleDiv != null) {
        index = subtripScheduleDiv.length;
        space = "";
    }
    const div = document.createElement("div");
    div.className = "subtripScheduleDiv:Subtrip-" + subtripPosition + space;
    var scheduleNumber = index + 1;
    div.innerHTML = `
                           
                    <h6>Horário `+ scheduleNumber + `</h6>
                    <div>
                        <input hidden class="form-control" type="text" asp-for="SubtripModelList[`+ subtripPosition + `].ma16subtripscheduleList[` + index + `].FK1614idsubtrip" value="` + subtripID + `" required />
                    </div>
                    <div>
                        <label>Dias do Horário</label>
                        <input class="form-control" type="text" name="SubtripModelList[`+ subtripPosition + `].ma16subtripscheduleList[` + index + `].ma16days" required/>
                    </div>
                    <div>
                        <label>Duração do Horário</label>
                        <input class="form-control" type="time" name="SubtripModelList[`+ subtripPosition + `].ma16subtripscheduleList[` + index + `].ma16duration" required />
                    </div>
                    <div>
                        <label>Horário de Entrada</label>
                        <input class="form-control" type="time" name="SubtripModelList[`+ subtripPosition + `].ma16subtripscheduleList[` + index + `].ma16entry" required />
                    </div>
                    <div>
                        <label>Horário de Saída</label>
                        <input class="form-control" type="time" name="SubtripModelList[`+ subtripPosition + `].ma16subtripscheduleList[` + index + `].ma16exit" />
                    </div>
                            <hr />
                    `;
    subtripScheduleListDiv.appendChild(div);
}