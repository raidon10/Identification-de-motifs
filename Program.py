import pyro
import pyro.distributions as dist
import torch
import random
from pyro.infer import SVI, TraceEnum_ELBO
from pyro.infer.autoguide import AutoNormal
from pyro import poutine
from pyro.infer import config_enumerate

def generate_sequences(num_sequences, sequence_length, hidden_motif):
    nucleotides = ['A', 'C', 'T', 'G']
    sequences = []
    motif_positions = []
    
    for _ in range(num_sequences):
        include_motif = random.random() > 0.3  # 70% de chance d'inclure le motif
        sequence = [random.choice(nucleotides) for _ in range(sequence_length)]
        
        if include_motif:
            motif_pos = random.randint(0, sequence_length - len(hidden_motif))
            motif_positions.append(motif_pos)
            for j, char in enumerate(hidden_motif):
                sequence[motif_pos + j] = char
        else:
            motif_positions.append(-1)
        
        sequences.append("".join(sequence))
    
    return sequences, motif_positions

@config_enumerate
def motif_model(sequences, motif_length):
    sequence_length = len(sequences[0])
    motif_position = pyro.sample("motif_position", dist.Categorical(torch.ones(sequence_length - motif_length + 1)))
    
    for i, sequence in enumerate(sequences):
        pyro.sample(f"obs_{i}", dist.Categorical(torch.ones(sequence_length)), obs=torch.tensor(["ACTG".index(c) for c in sequence]))

# Fonction principale
def main():
    hidden_motif = input("Entrez le motif à rechercher : ").strip().upper()
    if not all(c in 'ACTG' for c in hidden_motif):
        print("Le motif doit être composé uniquement des lettres A, C, T, G.")
        return
    
    motif_length = len(hidden_motif)
    num_sequences = random.randint(20, 50)
    sequence_length = 25
    sequences, motif_positions = generate_sequences(num_sequences, sequence_length, hidden_motif)
    
    print("\nSéquences ADN générées :")
    for i, seq in enumerate(sequences):
        motif_pos = motif_positions[i]
        end_pos = motif_pos + motif_length - 1 if motif_pos != -1 else -1
        print(f"{i + 1}: {seq} (Motif: {motif_pos} - {end_pos})")
    
    guide = AutoNormal(poutine.block(motif_model, hide=["motif_position"]))
    svi = SVI(motif_model, guide, pyro.optim.Adam({"lr": 0.01}), loss=TraceEnum_ELBO(max_plate_nesting=1))
    print("Modèle de détection de motif prêt pour l'inférence.")


if __name__ == "__main__":
    main()
