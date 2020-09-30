function addDiscount() {
    var discountListDiv = document.getElementById("discountListDiv");
    var dicountDiv = document.getElementsByClassName("dicountDiv");
    var index = 0;
    if (dicountDiv != null) {
        index = dicountDiv.length;
    }
    const div = document.createElement("div");
    var discountNumber = index + 1;
    div.className = "dicountDiv";
    div.innerHTML = `<h6>Desconto ` + discountNumber +`</h6>
                <div>
                    <label>Nome do Desconto (Obrigatório)</label>
                    <input type="text" class="form-control" name="ma27AgeDiscountList[`+ index +`].ma27name" required />
                </div>
                <div>
                    <label>Porcentagem de Desconto (Obrigatório)</label>
                    <input type="number" class="form-control" name="ma27AgeDiscountList[`+ index +`].ma27DiscountPercent" required />
                </div>
                <div>
                    <label>Idade Mínima (Padrão 0)</label>
                    <input type="number" class="form-control" name="ma27AgeDiscountList[`+ index +`].ma27minage" value="0" min="0" />
                </div>
                <div>
                    <label>Idade Máxima (Opcional)</label>
                    <input type="number" class="form-control" name="ma27AgeDiscountList[`+ index +`].ma27maxage" />
                </div>
                <div>
                    <label>Quantidade Mínima de Pessoas (Obrigatório)</label>
                    <input type="number" class="form-control" name="ma27AgeDiscountList[`+ index +`].ma27minPeople" required />
                </div>
                <div>
                    <label>Responsável?</label>
                    <select class="form-control" name="ma27AgeDiscountList[`+ index +`].ma27guardian" required>
                        <option value="false">Não</option>
                        <option value="true">Sim</option>
                    </select>
                </div>
                <div>
                    <label>Status do Item</label>
                    <select class="form-control" name="ma27AgeDiscountList[`+ index +`].ma27status">
                            <option value="1">Publicar</option>
                            <option value="0">Não Publicar</option>
                    </select>
                </div>
                <hr />`;
    discountListDiv.appendChild(div);
}

function addSubtrip() {
    var subtripListDiv = document.getElementById("subtripListDiv");
    var subtripDiv = document.getElementsByClassName("subtripDiv");
    var index = 0;
    var space = "";
    if (subtripDiv != null) {
        index = subtripDiv.length;
        //space = space + " mt-5";
    }
    const div = document.createElement("div");
    div.className = "subtripDiv";
    var subtripNumber = index + 1;
    div.innerHTML = `
                <div class="mb-3">
                    <h6>Passeio `+ subtripNumber +`</h6>
                    <div>
                        <label>Nome do Passeio</label>
                        <input type="text" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14name" required />
                    </div>

                    
                    <div class="row">
                        <div class="col-6">
                            <label>Endereço do Passeio</label>
                            <input type="text" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14address" required />
                        </div>
                        <div class="col-6">
                            <label>Bairro do Passeio</label>
                            <input type="text" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14neighborhood" required />
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-6">
                            <label>Vaga Por Passeio</label>
                            <input type="number" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14vacancy" required />
                        </div>
                        <div class="col-6">
                            <label>Quantidade Mínima de Pessoas Por Passeio</label>
                            <input type="number" class="form-control" name="SubtripModelList[`+ index +`].ma14subtrip.ma14minPeopleQuantity" required />
                        </div>
                        <div class="col-6">
                            <label>Quantidade de Grupos Por Passeio</label>
                            <input type="number" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14groupnumber" required />
                        </div>
                    </div>
                    
                    
                    <div>
                        <label>Descrição do Passeio</label>
                        <textarea class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14Description" required ></textarea>
                    </div>
                    <div>
                        <label>Aceita Cupom de Desconto?</label>
                        <select class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14InfluencerDiscount" required>
                            <option value="0">Não</option>
                            <option value="1">Sim</option>
                        </select>
                    </div>

                    <div class="row">
                        <div class="col-4">
                            <label>Desconto do Parceiro (Opcional)</label>
                            <input type="number" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14PartnerDiscountPercent" min="0" value="0" required />
                        </div>
                        <div class="col-4">
                            <label>Início do Desconto do Parceiro (Opcional)</label>
                            <input type="date" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14InitialDiscountDate" />
                        </div>
                        <div class="col-4">
                            <label>Final do Desconto do Parceiro (Opcional)</label>
                            <input type="date" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14FinalDiscountDate" />
                        </div>
                    </div>
                

                    <div>
                        <label>Comissão Da Matrip</label>
                        <input type="number" class="form-control" name="SubtripModelList[`+ index + `].ma14subtrip.ma14MatripCommission" min="0" value="0" required />
                    </div>
                    <div>
                        <label>Nome da Empresa Deste Passeio</label>
                        <input id="PartnerName-`+ index + `" class="form-control" type="text" name="SubtripModelList[` + index + `].PartnerName" placeholder="Nome da empresa" onkeydown="PartnerAutoComplete(` + index + `);" required />
                    </div>
                    <div>
                        <label>Status do Item</label>
                        <select class="form-control" name="SubtripModelList[`+ index +`].ma14subtrip.ma14status">
                            <option value="1">Publicar</option>
                            <option value="0">Não Publicar</option>
                        </select>
                    </div>
                </div>


                <div class="row card-deck">
                    
                    <div class="col-4">
                        <div id="serviceListDiv:Subtrip-`+ index + `">
                            <a class="btn btn-outline-success" onclick="addService(`+ index + `);">Criar Serviço</a>
                            <a class="btn btn-outline-success" onclick="ExcludeItem(0, 'serviceDiv:Subtrip-`+ index + `');">Excluir Item</a>
                            <hr />
                        
                        </div>
                    </div>
                    <div class="col-4">
                        <div id="subtripValueListDiv:Subtrip-`+ index + `">
                            <a class="btn btn-outline-success" onclick="addSubtripValue(`+ index + `);">Criar Valor</a>
                            <a class="btn btn-outline-success" onclick="ExcludeItem(1, 'subtripValueDiv:Subtrip-`+ index + `');">Excluir Item</a>
                            <hr />
                            <div class="subtripValueDiv:Subtrip-`+ index + `">
                                <h6>Valor 1</h6>
                                <div>
                                    <label>Valor</label>
                                    <input class="form-control money" name="SubtripModelList[`+ index + `].ma17SubtripValueList[0].ma17value" required />
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
                                    </select>
                                </div>
                                <div>
                                    <label>Status do Item</label>
                                    <select class="form-control" name="SubtripModelList[`+ index +`].ma17SubtripValueList[0].ma17status">
                                        <option value="1">Publicar</option>
                                        <option value="0">Não Publicar</option>
                                    </select>
                                </div>
                                <hr />
                            </div>
                        </div>
                    </div>

                
                
                    <div class="col-4">
                        <div id="subtripScheduleListDiv:Subtrip-`+ index + `">
                            <a class="btn btn-outline-success" onclick="addSubtripSchedule(`+ index + `);">Criar Horário</a>
                            <a class="btn btn-outline-success" onclick="ExcludeItem(1, 'subtripScheduleDiv:Subtrip-`+ index + `');">Excluir Item</a>
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
                                    <input class="form-control" type="time" name="SubtripModelList[`+ index + `].ma16subtripscheduleList[0].ma16exit" required />
                                </div>
                                <div>
                                    <label>Status do Item</label>
                                    <select class="form-control" name="SubtripModelList[`+ index +`].ma16subtripscheduleList[0].ma16status">
                                            <option value="1">Publicar</option>
                                            <option value="0">Não Publicar</option>
                                    </select>
                                </div>
                                <hr />
                            </div>
                        </div>
                    </div>

                </div>

                
                    

                <hr/>`;
    subtripListDiv.appendChild(div);
}

function addService(subtripPosition) {
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
                                <input class="form-control money" name="SubtripModelList[`+ subtripPosition + `].ma11serviceList[` + index + `].ma11Value"  required />
                            </div>
                            <div>
                                <label>Status do Item</label>
                                <select class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma11serviceList[` + index +`].ma11status">
                                    <option value="1">Publicar</option>
                                    <option value="0">Não Publicar</option>
                                </select>
                            </div>
                            <hr />
                    `;
    serviceListDiv.appendChild(div);
}
function addSubtripValue(subtripPosition) {
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
                           
                    <h6>Valor `+ valueNumber+`</h6>
                    <div>
                        <label>Valor</label>
                        <input class="form-control money" name="SubtripModelList[`+ subtripPosition + `].ma17SubtripValueList[` + index + `].ma17value"  required />
                    </div>
                    <select class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma17SubtripValueList[` + index + `].ma17description" required>
                        <option value="Individual">Individual</option>
                        <option value="Privativo">Privativo</option>
                        <option value="Pacote">Pacote</option>
                    </select>
                    <div>
                        <label>Status do Valor</label>
                        <select class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma17SubtripValueList[` + index + `].ma17type" required>
                            <option value="0">Individual</option>
                            <option value="1">Privativo</option>
                        </select>
                    </div>
                    <div>
                        <label>Status do Item</label>
                        <select class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma17SubtripValueList[` + index + `].ma17status">
                            <option value="1">Publicar</option>
                            <option value="0">Não Publicar</option>
                        </select>
                    </div>
                    <hr />  
                    `;
    subtripValueListDiv.appendChild(div);
}

function addSubtripSchedule(subtripPosition) {
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
                        <input class="form-control" type="time" name="SubtripModelList[`+ subtripPosition + `].ma16subtripscheduleList[` + index + `].ma16exit" required />
                    </div>
                    <div>
                        <label>Status do Item</label>
                        <select class="form-control" name="SubtripModelList[`+ subtripPosition + `].ma16subtripscheduleList[` + index +`].ma16status">
                                <option value="1">Publicar</option>
                                <option value="0">Não Publicar</option>
                        </select>
                    </div>
                    <hr />
                    `;
    subtripScheduleListDiv.appendChild(div);
}

function ExcludeItem(minDiv, itemName) {
    var itemList = document.getElementsByClassName(itemName);

    if (itemList.length > minDiv) {
        itemList[itemList.length - 1].remove();
    }
}