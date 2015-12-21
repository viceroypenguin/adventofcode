<Query Kind="Program">
  <GACReference>System.Collections.Immutable, Version=1.1.37.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</GACReference>
  <GACReference>System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</GACReference>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

void Main()
{
	var input =
@"Al => ThF
Al => ThRnFAr
B => BCa
B => TiB
B => TiRnFAr
Ca => CaCa
Ca => PB
Ca => PRnFAr
Ca => SiRnFYFAr
Ca => SiRnMgAr
Ca => SiTh
F => CaF
F => PMg
F => SiAl
H => CRnAlAr
H => CRnFYFYFAr
H => CRnFYMgAr
H => CRnMgYFAr
H => HCa
H => NRnFYFAr
H => NRnMgAr
H => NTh
H => OB
H => ORnFAr
Mg => BF
Mg => TiMg
N => CRnFAr
N => HSi
O => CRnFYFAr
O => CRnMgAr
O => HP
O => NRnFAr
O => OTi
P => CaP
P => PTi
P => SiRnFAr
Si => CaSi
Th => ThCa
Ti => BP
Ti => TiTi
e => HF
e => NAl
e => OMg

CRnSiRnCaPTiMgYCaPTiRnFArSiThFArCaSiThSiThPBCaCaSiRnSiRnTiTiMgArPBCaPMgYPTiRnFArFArCaSiRnBPMgArPRnCaPTiRnFArCaSiThCaCaFArPBCaCaPTiTiRnFArCaSiRnSiAlYSiThRnFArArCaSiRnBFArCaCaSiRnSiThCaCaCaFYCaPTiBCaSiThCaSiThPMgArSiRnCaPBFYCaCaFArCaCaCaCaSiThCaSiRnPRnFArPBSiThPRnFArSiRnMgArCaFYFArCaSiRnSiAlArTiTiTiTiTiTiTiRnPMgArPTiTiTiBSiRnSiAlArTiTiRnPMgArCaFYBPBPTiRnSiRnMgArSiThCaFArCaSiThFArPRnFArCaSiRnTiBSiThSiRnSiAlYCaFArPRnFArSiThCaFArCaCaSiThCaCaCaSiRnPRnCaFArFYPMgArCaPBCaPBSiRnFYPBCaFArCaSiAl

";

	var regex = new Regex(@"^(((\w+) => (\w+))|(\w+))$");

	var matches = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select(s => regex.Match(s))
		.ToList();


	var transformations = matches.Where(m => m.Groups[2].Success)
		.Select(m => new Transformation()
		{
			Source = m.Groups[3].Value,
			Result = m.Groups[4].Value,
		})
		.ToList();

	var molecule = matches.Where(m => m.Groups[5].Success)
		.Select(m => m.Groups[5].Value)
		.Single();

	GetTransformations(
		molecule, 
		transformations,
		ImmutableStack<TransformationResult>.Empty
			.Push(new TransformationResult()
			{
				Source = "",
				Result = "",
				Molecule = molecule,
			}))
		.Dump();

}

class Transformation
{
	public string Source { get; set; }
	public string Result { get; set; }
}

class TransformationResult
{
	public string Source { get; set; }
	public string Result { get; set; }
	public string Molecule { get; set; }
}

ImmutableStack<TransformationResult> GetTransformations(string molecule, List<Transformation> language, ImmutableStack<TransformationResult> stack)
{
	foreach (var opt in language)
	{
		var idx = molecule.IndexOf(opt.Result);
        if (idx < 0)
			continue;

		var newMolecule = molecule.Substring(0, idx) + opt.Source + molecule.Substring(idx + opt.Result.Length);
		var newStack = stack.Push(new TransformationResult
		{
			Source = opt.Source,
			Result = opt.Result,
			Molecule = newMolecule,
		});
		if (newMolecule == "e")
			return newStack;

		newStack = GetTransformations(newMolecule, language, newStack);
		if (newStack != null)
			return newStack;
	}

	return null;
}
