import api from '@/services/api';

const state = {
    tarefas: [],
    gruposTarefas: [],
};

const getters = {
    allTasks: state => state.tarefas,
    allGroups: state => state.gruposTarefas,
};

const actions = {
    async getGruposTarefas({ commit }) {
        try {
            const response = await api.get('GrupoTarefas');
            commit('setGruposTarefas', response.data);
        } catch (error) {
            console.error(error);
        }
    },

    async addGrupoTarefas({ commit }, grupoTarefasData) {
        try {
            const response = await api.post('GrupoTarefas', {
                Nome: grupoTarefasData.nome,
            });
            commit('addGrupoTarefas', response.data);
        } catch (error) {
            console.error(error);
            throw error;
        }
    },

    async addTarefa({ commit }, tarefaData) {
        try {
            console.log(tarefaData);
            const response = await api.post('Tarefa', {
                Nome: tarefaData.nome,
                Descricao: tarefaData.descricao,
                DataVencimento: tarefaData.dataVencimento,
                Status: false,
                GrupoTarefasId: tarefaData.grupoTarefasId,
            });
            commit('addTarefa', response.data);
        } catch (error) {
            console.error(error);
            throw error;
        }
    },

    async atualizarTarefa({ commit }, tarefaData) {
        try {
            console.log(tarefaData);
            const response = await api.put(`Tarefa/${tarefaData.id}`, {
                Id: tarefaData.id,
                Nome: tarefaData.nome,
                Descricao: tarefaData.descricao,
                DataVencimento: tarefaData.dataVencimento,
                GrupoTarefasId: tarefaData.grupoTarefasId,
                Status: tarefaData.status,
                Conclusao: tarefaData.conclusao,
            });
            commit('atualizarTarefa', response.data);
        } catch (error) {
            console.error(error);
            throw error;
        }
    },
};

const mutations = {
    setGruposTarefas(state, gruposTarefas) {
        state.gruposTarefas = gruposTarefas;
    },

    addGrupoTarefas(state, grupoTarefa) {
        state.gruposTarefas.push(grupoTarefa);
    },

    addTarefa(state, tarefa) {
        state.tarefas.push(tarefa);
    },

    atualizarTarefa(state, tarefaAtualizada) {
        const index = state.tarefas.findIndex(tarefa => tarefa.id === tarefaAtualizada.id);
        if (index !== -1) {
            state.tarefas.splice(index, 1, tarefaAtualizada); 
        }
    },
};

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations,
};
