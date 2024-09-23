<template>
  <v-app>
    <v-main>
      <v-container class="py-5">
        <v-row justify="center" class="py-8">
          <v-btn height="72" min-width="164" @click="dialogoCriacao = true">
            Adicionar Grupo
          </v-btn>
        </v-row>

        <v-dialog v-model="dialogoCriacao" max-width="350">
          <v-card>
            <v-card-title class="pt-8">
              <span class="font-weight-light text-h5 pl-3">
                Novo grupo de tarefas
              </span>
            </v-card-title>

            <v-card-text>
              <v-form ref="formularioGrupo" v-model="formularioGrupoValido">
                <v-text-field
                    label="Nome"
                    v-model="dadosFormulario.nome"
                    :rules="[regras.obrigatorio]"
                    required
                ></v-text-field>
              </v-form>
            </v-card-text>

            <v-card-actions>
              <v-btn color="primary" @click="criarGrupo">Criar</v-btn>
              <v-btn text @click="fecharDialogoCriacao">Cancelar</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

        <div v-if="gruposTarefas.length == 0" class="text-center">
          <p>Nenhum grupo disponível.</p>
        </div>

        <v-list-item v-for="grupo in gruposTarefas" :key="grupo.id">
          <v-container>
            <v-row justify="center" class="py-8">
              <v-btn
                  style="margin-left: 30px;"
                  height="72"
                  min-width="164"
                  @click="abrirDialogoAdicionarTarefa(grupo)"
              >
                Adicionar Tarefa - {{ grupo.nome }}
              </v-btn>
            </v-row>

            <v-row justify="center">
              <v-list-item v-for="tarefa in grupo.tarefas" :key="tarefa.id">
                <v-col cols="auto">
                  <v-card
                      style="padding: 16px 32px 16px 32px;"
                      class="mx-auto"
                      max-width="400px"
                      :style="{ backgroundColor: tarefa.status ? 'rgb(170, 223, 170)' : '' }"
                  >
                    <v-card-text>
                      <div style="font-weight: 100;">
                        Data de vencimento <br />
                        {{ formatarDataTarefa(tarefa.dataVencimento) }}
                      </div>
                      <p style="padding-top:30px;" class="text-h4 font-weight-black">
                        {{ tarefa.nome }}
                      </p>
                      <p style="padding-bottom:20px">{{ tarefa.descricao }}</p>
                      <div style="font-weight: 100;">
                        <span v-if="tarefa.modificacao">
                          Data de modificação <br />
                          {{ formatarDataTarefa(tarefa.modificacao) }}
                        </span>
                      </div>
                      <div style="font-weight: 100;">
                        <span v-if="tarefa.criacao">
                          Data de criacao <br />
                          {{ formatarDataTarefa(tarefa.criacao) }}
                        </span>
                      </div>
                      <div style="font-weight: 100;">
                        <span v-if="tarefa.conclusao">
                          Data de conclusão <br />
                          {{ formatarDataTarefa(tarefa.conclusao) }}
                        </span>
                      </div>
                    </v-card-text>

                    <v-card-actions style="height:20px;">
                      <v-checkbox
                          style="padding-top: 20px;"
                          v-model="tarefa.status"
                          @change="onStatusChange(tarefa)"
                      ></v-checkbox>

                      <v-btn
                          color="teal-accent-4"
                          variant="text"
                          @click="abrirDialogoEdicao(tarefa)"
                      >
                        Editar
                      </v-btn>
                    </v-card-actions>
                  </v-card>
                </v-col>
              </v-list-item>
            </v-row>
          </v-container>
        </v-list-item>

        <v-dialog v-model="dialogoEdicao" max-width="400" @after-leave="resetTarefaAtual">
          <v-card v-if="tarefaAtual">
            <v-card-title class="pt-8">
              <span class="font-weight-light text-h5 pl-3">Editar Tarefa</span>
            </v-card-title>

            <v-card-text>
              <v-form ref="formularioTarefa" v-model="formularioTarefaValido">
                <v-text-field
                    label="Nome da Tarefa"
                    v-model="tarefaAtual.nome"
                    :rules="[regras.obrigatorio]"
                    required
                ></v-text-field>

                <v-text-field
                    label="Data de Vencimento"
                    v-model="dataHoraFormatada"
                    prepend-icon="mdi-calendar"
                    @input="aoInserirDataHora"
                    @keydown="aoPressionarTecla"
                    :rules="[regras.dataHora]"
                    required
                    maxlength="16"
                    inputmode="numeric"
                ></v-text-field>

                <v-textarea
                    label="Descrição"
                    v-model="tarefaAtual.descricao"
                    :rules="[regras.obrigatorio]"
                    rows="3"
                    required
                ></v-textarea>
              </v-form>
            </v-card-text>

            <v-card-actions>
              <v-btn color="primary" @click="salvarTarefa">Salvar</v-btn>
              <v-btn text @click="fecharDialogoEdicao">Cancelar</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

        <v-dialog v-model="dialogoAdicionarTarefa" max-width="400" @after-leave="resetNovaTarefa">
          <v-card>
            <v-card-title class="pt-8">
              <span class="font-weight-light text-h5 pl-3">Nova Tarefa</span>
            </v-card-title>

            <v-card-text>
              <v-form ref="formularioNovaTarefa" v-model="formularioNovaTarefaValido">
                <v-text-field
                    label="Nome da Tarefa"
                    v-model="novaTarefa.nome"
                    :rules="[regras.obrigatorio]"
                    required
                ></v-text-field>

                <v-text-field
                    label="Data de Vencimento"
                    v-model="novaTarefaDataHoraFormatada"
                    prepend-icon="mdi-calendar"
                    @input="aoInserirDataHoraNovaTarefa"
                    @keydown="aoPressionarTecla"
                    :rules="[regras.dataHora]"
                    required
                    maxlength="16"
                    inputmode="numeric"
                ></v-text-field>

                <v-textarea
                    label="Descrição"
                    v-model="novaTarefa.descricao"
                    :rules="[regras.obrigatorio]"
                    rows="3"
                    required
                ></v-textarea>
              </v-form>
            </v-card-text>

            <v-card-actions>
              <v-btn color="primary" @click="salvarNovaTarefa">Salvar</v-btn>
              <v-btn text @click="fecharDialogoAdicionarTarefa">Cancelar</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

        <v-snackbar v-model="notificacao" :timeout="3000" color="error">
          {{ textoNotificacao }}
        </v-snackbar>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import { mapState, mapActions } from 'vuex';

export default {
  name: 'ListaDeGrupos',
  data() {
    return {
      dialogoCriacao: false,
      dialogoEdicao: false,
      dialogoAdicionarTarefa: false,
      formularioGrupoValido: false,
      formularioTarefaValido: false,
      formularioNovaTarefaValido: false,
      dadosFormulario: {
        nome: '',
      },
      tarefaAtual: null,
      grupoAtual: null,
      novaTarefa: {
        nome: '',
        descricao: '',
        dataVencimento: '',
      },
      dataHoraFormatada: '',
      novaTarefaDataHoraFormatada: '',
      regras: {
        obrigatorio: (valor) => (valor && valor.trim ? !!valor.trim() : false) || 'Obrigatório*',
        dataHora: (valor) =>
            this.validarDataHora(valor) || 'Data inválida ou no passado (dd/mm/aaaa hh:mm)',
      },
      notificacao: false,
      textoNotificacao: '',
      estaDeletando: false,
    };
  },
  computed: {
    ...mapState('tasks', {
      gruposTarefas: (state) => state.gruposTarefas,
    }),
  },
  methods: {
    ...mapActions('tasks', [
      'getGruposTarefas',
      'addGrupoTarefas',
      'atualizarTarefa',
      'addTarefa',
    ]),

    onStatusChange(tarefa) {
      if (tarefa.status) {
        tarefa.conclusao = new Date();
      } else {
        tarefa.conclusao = null;
      }
      this.atualizarTarefa(tarefa);
      this.getGruposTarefas();
    },

    formatarDataTarefa(data) {
      if (data) {
        return this.formatarDataHora(new Date(data));
      }
      return '';
    },

    fecharDialogoCriacao() {
      this.dialogoCriacao = false;
      if (this.$refs.formularioGrupo) {
        this.$refs.formularioGrupo.reset();
      }
      this.formularioGrupoValido = false;
    },

    abrirDialogoAdicionarTarefa(grupo) {
      this.grupoAtual = grupo;
      this.dialogoAdicionarTarefa = true;
    },

    fecharDialogoAdicionarTarefa() {
      this.dialogoAdicionarTarefa = false;
      if (this.$refs.formularioNovaTarefa) {
        this.$refs.formularioNovaTarefa.reset();
      }
      this.formularioNovaTarefaValido = false;
      this.novaTarefa = {
        nome: '',
        descricao: '',
        dataVencimento: '',
      };
      this.novaTarefaDataHoraFormatada = '';
    },

    abrirDialogoEdicao(tarefa) {
      this.tarefaAtual = JSON.parse(JSON.stringify(tarefa));
      if (this.tarefaAtual.dataVencimento) {
        const dataObj = new Date(this.tarefaAtual.dataVencimento);
        this.dataHoraFormatada = this.formatarDataHora(dataObj);
      } else {
        this.dataHoraFormatada = '';
      }
      this.dialogoEdicao = true;
    },

    fecharDialogoEdicao() {
      this.dialogoEdicao = false;
      if (this.$refs.formularioTarefa) {
        this.$refs.formularioTarefa.reset();
      }
      this.formularioTarefaValido = false;
    },

    resetTarefaAtual() {
      this.tarefaAtual = null;
    },

    resetNovaTarefa() {
      this.novaTarefa = {
        nome: '',
        descricao: '',
        dataVencimento: '',
      };
      this.novaTarefaDataHoraFormatada = '';
    },

    aoPressionarTecla(evento) {
      this.estaDeletando = evento.key === 'Backspace' || evento.key === 'Delete';
    },

    aoInserirDataHora(evento) {
      this.formatarCampoDataHora(evento, 'dataHoraFormatada', 'tarefaAtual');
    },

    aoInserirDataHoraNovaTarefa(evento) {
      this.formatarCampoDataHora(evento, 'novaTarefaDataHoraFormatada', 'novaTarefa');
    },

    formatarCampoDataHora(evento, campoFormatado, objetoTarefa) {
      const input = evento.target;
      const valor = input.value;
      let posicaoCursor = input.selectionStart;

      let numeros = valor.replace(/\D/g, '');
      if (numeros.length > 12) numeros = numeros.slice(0, 12);

      let posicaoCursorNumeros = 0;
      for (let i = 0; i < posicaoCursor; i++) {
        if (/\d/.test(valor[i])) {
          posicaoCursorNumeros++;
        }
      }

      let valorFormatado = '';
      const posicoes = [2, 4, 8, 10];
      const separadores = ['/', '/', ' ', ':'];
      let indiceNumeros = 0;

      for (let i = 0; indiceNumeros < numeros.length; i++) {
        if (posicoes.includes(i)) {
          valorFormatado += separadores[posicoes.indexOf(i)];
        }
        valorFormatado += numeros[indiceNumeros++];
      }

      this[campoFormatado] = valorFormatado;

      let novaPosicaoCursor = 0;
      let numerosPercorridos = 0;
      for (let i = 0; i < valorFormatado.length; i++) {
        if (/\d/.test(valorFormatado[i])) {
          numerosPercorridos++;
          if (numerosPercorridos > posicaoCursorNumeros) {
            break;
          }
        }
        novaPosicaoCursor++;
      }

      this.$nextTick(() => {
        input.setSelectionRange(novaPosicaoCursor, novaPosicaoCursor);
      });

      this.estaDeletando = false;

      if (numeros.length === 12) {
        if (this.validarDataHora(this[campoFormatado])) {
          const dataObj = this.analisarDataHora(this[campoFormatado]);
          if (dataObj) {
            this[objetoTarefa].dataVencimento = dataObj;
          }
        }
      }
    },

    validarDataHora(valor) {
      const regexDataHora = /^(\d{2})\/(\d{2})\/(\d{4}) (\d{2}):(\d{2})$/;
      if (!regexDataHora.test(valor)) {
        return false;
      }

      const [_, diaStr, mesStr, anoStr, horasStr, minutosStr] = regexDataHora.exec(valor);
      const dia = parseInt(diaStr, 10);
      const mes = parseInt(mesStr, 10);
      const ano = parseInt(anoStr, 10);
      const horas = parseInt(horasStr, 10);
      const minutos = parseInt(minutosStr, 10);

      if (
          dia < 1 ||
          dia > 31 ||
          mes < 1 ||
          mes > 12 ||
          ano < 1900 ||
          ano > 2100 ||
          horas < 0 ||
          horas > 23 ||
          minutos < 0 ||
          minutos > 59
      ) {
        return false;
      }

      const dataObj = new Date(ano, mes - 1, dia, horas, minutos);
      if (
          dataObj.getFullYear() !== ano ||
          dataObj.getMonth() !== mes - 1 ||
          dataObj.getDate() !== dia
      ) {
        return false;
      }

      const dataAtual = new Date();
      if (dataObj <= dataAtual) {
        return false;
      }

      return true;
    },

    analisarDataHora(valor) {
      const [parteData, parteHora] = valor.split(' ');
      const [dia, mes, ano] = parteData.split('/').map(Number);
      const [horas, minutos] = parteHora.split(':').map(Number);

      const dataObj = new Date(ano, mes - 1, dia, horas, minutos);

      if (
          dataObj &&
          dataObj.getFullYear() === ano &&
          dataObj.getMonth() === mes - 1 &&
          dataObj.getDate() === dia
      ) {
        return dataObj;
      }
      return null;
    },

    formatarDataHora(data) {
      const dd = String(data.getDate()).padStart(2, '0');
      const mm = String(data.getMonth() + 1).padStart(2, '0');
      const yyyy = data.getFullYear();
      const hh = String(data.getHours()).padStart(2, '0');
      const min = String(data.getMinutes()).padStart(2, '0');
      return `${dd}/${mm}/${yyyy} ${hh}:${min}`;
    },

    async criarGrupo() {
      if (this.formularioGrupoValido) {
        try {
          await this.addGrupoTarefas({
            nome: this.dadosFormulario.nome,
          });
          this.fecharDialogoCriacao();
          this.dadosFormulario.nome = '';
          this.getGruposTarefas();
        } catch (erro) {
          console.error('Erro ao criar grupo:', erro);
        }
      } else {
        this.textoNotificacao = 'Preencha o formulário corretamente.';
        this.notificacao = true;
      }
    },

    async salvarTarefa() {
      if (this.formularioTarefaValido) {
        try {
          const grupo = this.gruposTarefas.find((g) =>
              g.tarefas.some((t) => t.id === this.tarefaAtual.id)
          );
          if (grupo) {
            const index = grupo.tarefas.findIndex((t) => t.id === this.tarefaAtual.id);
            if (index !== -1) {
              if (this.validarDataHora(this.dataHoraFormatada)) {
                const dataObj = this.analisarDataHora(this.dataHoraFormatada);
                if (dataObj) {
                  this.tarefaAtual.dataVencimento = dataObj.toISOString();
                }
              }
              
              await this.atualizarTarefa(this.tarefaAtual);
              this.getGruposTarefas();
            }
          }
          this.fecharDialogoEdicao();
        } catch (erro) {
          console.error('Erro ao salvar tarefa:', erro);
        }
      } else {
        this.textoNotificacao = 'Preencha o formulário corretamente.';
        this.notificacao = true;
      }
    },

    async salvarNovaTarefa() {
      if (this.formularioNovaTarefaValido) {
        try {
          if (this.validarDataHora(this.novaTarefaDataHoraFormatada)) {
            const dataObj = this.analisarDataHora(this.novaTarefaDataHoraFormatada);
            if (dataObj) {
              this.novaTarefa.dataVencimento = dataObj.toISOString();
            }
          }
          this.novaTarefa.grupoTarefasId = this.grupoAtual.id;
          await this.addTarefa(this.novaTarefa);
          this.fecharDialogoAdicionarTarefa();
          this.getGruposTarefas();
        } catch (erro) {
          console.error('Erro ao adicionar tarefa:', erro);
        }
      } else {
        this.textoNotificacao = 'Preencha o formulário corretamente.';
        this.notificacao = true;
      }
    },
  },
  watch: {
    dialogoEdicao(newVal) {
      if (!newVal) {
        this.tarefaAtual = null;
      }
    },
  },
  created() {
    this.getGruposTarefas();
  },
};
</script>

<style>
.text-decoration-line-through {
  text-decoration: line-through;
}
.text-center {
  text-align: center;
}
</style>
